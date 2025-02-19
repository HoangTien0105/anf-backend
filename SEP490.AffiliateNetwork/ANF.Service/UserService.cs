using ANF.Core;
using ANF.Core.Exceptions;
using ANF.Core.Models.Entities;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using ANF.Infrastructure;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ANF.Service
{
    public class UserService(IUnitOfWork unitOfWork,
                             TokenService tokenService,
                             IMapper mapper, 
                             IConfiguration configuration) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly TokenService _tokenService = tokenService;
        private readonly IMapper _mapper = mapper;
        private readonly IConfiguration _configuration = configuration;

        public async Task<LoginResponse> Login(string email, string password)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var user = await userRepository.GetAll().AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user is null) throw new KeyNotFoundException("User does not exist.");
            //if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
            //{
            //    throw new ForbiddenException("User's password is incorrect.");
            //}
            var response = _mapper.Map<LoginResponse>(user);
            response.AccessToken = _tokenService.GenerateToken(user);
            return response;
        }

        public LoginResponse LoginForAdmin(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw new ArgumentNullException("Email or password is null or empty.");
            var adminConfig = _configuration.GetSection("Admin");
            var adminEmail = adminConfig["Email"];
            var adminPassword = adminConfig["Password"];

            if (email != adminEmail || password != adminPassword)
                throw new ForbiddenException("Admin's credentials are incorrect.");
            var admin = new User
            {
                //Id = Guid.NewGuid(),
                Email = adminEmail,
                Role = Core.Enums.UserRoles.Admin,
            };
            return new LoginResponse
            {
                //Id = admin.Id,
                FirstName = "ADMIN",
                LastName = "ADMIN",
                AccessToken = _tokenService.GenerateToken(admin),
            };
        }

        public async Task<bool> RegisterPublisher(PublisherCreateRequest request)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                if (request is null) throw new ArgumentException("Request data is nullable.");
                if (request.Password != request.PasswordConfirmed)
                    throw new ArgumentException("Passwords do not match.");

                var duplicatedUser = await userRepository.GetAll().AsNoTracking()
                    .AnyAsync(u => u.Email == request.Email);
                if (duplicatedUser) throw new DuplicatedException("User already exists.");

                var user = _mapper.Map<User>(request);
                userRepository.Add(user);
                var affectedRows = await _unitOfWork.SaveAsync();
                return affectedRows > 0;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
