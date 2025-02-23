using ANF.Core;
using ANF.Core.Enums;
using ANF.Core.Exceptions;
using ANF.Core.Models.Entities;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ANF.Service
{
    public class UserService(IUnitOfWork unitOfWork,
                             TokenService tokenService,
                             IMapper mapper) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly TokenService _tokenService = tokenService;
        private readonly IMapper _mapper = mapper;

        public async Task<LoginResponse> Login(string email, string password)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var user = await userRepository.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            if (user is null) throw new KeyNotFoundException("User does not exist.");
            if (user.Status == UserStatus.Deactive)
            {
                throw new UnauthorizedAccessException("User's account has been deactivated! Please contact to the IT support.");
            }

            var response = _mapper.Map<LoginResponse>(user);
            response.AccessToken = _tokenService.GenerateToken(user);
            return response;
        }

        public async Task<bool> RegisterAccount(AccountCreateRequest request)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                if (request is null) throw new NullReferenceException("Invalid request data. Please check again!");
                if (request.Password != request.PasswordConfirmed)
                    throw new ArgumentException("Passwords do not match.");
                if (!Enum.TryParse<UserRoles>(request.Role, true, out _))
                    throw new ArgumentException("Invalid user role. Please check again!");
                var age = DateTime.Today.Year - request.DateOfBirth.Year;
                if (age < 18)
                    throw new ArgumentException("You must be at least 18 years old to register!");
                var duplicatedUser = await userRepository.GetAll()
                    .AsNoTracking()
                    .AnyAsync(u => u.Email == request.Email);
                if (duplicatedUser) throw new DuplicatedException("User already exists.");

                var user = _mapper.Map<User>(request);
                userRepository.Add(user);
                var affectedRows = await _unitOfWork.SaveAsync();
                return affectedRows > 0;
            }
            catch
            {
                //await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<UserStatusResponse> UpdateAccountStatus(long userId, string status)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                if (!Enum.TryParse<UserStatus>(status, true, out _))
                    throw new ArgumentException("Invalid user's status. Please check again!");
                var user = await userRepository.FindByIdAsync(userId);
                if (user is null)
                    throw new KeyNotFoundException("User does not exist!");

                user.Status = Enum.Parse<UserStatus>(status, true);
                userRepository.Update(user);
                await _unitOfWork.SaveAsync();
                return new UserStatusResponse
                {
                    UserId = user.Id,
                    Status = user.Status.ToString(),
                };
            }
            catch
            {
                //await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
