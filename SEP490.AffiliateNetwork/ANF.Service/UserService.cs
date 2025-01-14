using ANF.Core;
using ANF.Core.Models.Entities;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using ANF.Infrastructure;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ANF.Service
{
    public class UserService(
        IUnitOfWork unitOfWork,
        TokenService tokenService,
        IMapper mapper) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly TokenService _tokenService = tokenService;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> CreateUser(PublisherCreateRequest request)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                if (request is null) throw new NullReferenceException("Request model is nullable.");
                if (request.Password != request.PasswordConfirmed)
                    throw new ArgumentException("Passwords do not match.");
                
                var publisher = _mapper.Map<User>(request);
                userRepository.Add(publisher);
                int result = await _unitOfWork.SaveAsync();
                return result > 0;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<LoginResponse> Login(string email, string password)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var user = await userRepository.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user is null) throw new KeyNotFoundException("User does not exist.");

            if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
                throw new UnauthorizedAccessException("User's password is incorrect.");
            var token = _tokenService.GenerateToken(user);

            return new LoginResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AccessToken = token
            };
        }
    }
}
