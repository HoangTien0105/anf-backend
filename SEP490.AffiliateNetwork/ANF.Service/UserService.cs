using ANF.Core;
using ANF.Core.Commons;
using ANF.Core.Enums;
using ANF.Core.Exceptions;
using ANF.Core.Models.Entities;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace ANF.Service
{
    public class UserService(IUnitOfWork unitOfWork,
                             TokenService tokenService,
                             IMapper mapper, IEmailService emailService) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly TokenService _tokenService = tokenService;
        private readonly IMapper _mapper = mapper;
        private readonly IEmailService _emailService = emailService;
        //NOTE: Change the application host when deploying successfully!
        private readonly string _appBaseUrl = "http://localhost:5272/api/affiliate-network";

        public async Task<bool> ChangeEmailStatus(long userId)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                var user = await userRepository.FindByIdAsync(userId);
                if (user is null)
                    throw new KeyNotFoundException("User does not exist!");
                if (user.EmailConfirmed == true)
                    throw new Exception("The email has already confirmed by the user!");    //TODO: Change the exception type
                // Email verification success
                user.EmailConfirmed = true;
                userRepository.Update(user);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch
            {
                //await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> ChangePassword(string email)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                var user = await userRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Email == email);
                if (user is null)
                    throw new KeyNotFoundException("User does not exist!");
                if (!string.IsNullOrEmpty(user.ResetPasswordToken))
                    throw new Exception("Fatal error! Reset token is not empty.");  //TODO: Change the exception type

                var token = Guid.NewGuid().ToString();
                user.ResetPasswordToken = token;
                user.ExpiryDate = DateTime.UtcNow.AddHours(1);
                userRepository.Update(user);

                var url = $"{_appBaseUrl}/users/{user.Id}/reset-token/{token}";
                var message = new EmailMessage
                {
                    To = user.Email,
                    Subject = "Reset password"
                };
                var result = await _emailService.SendTokenForResetPassword(message, url);
                if (result)
                    return await _unitOfWork.SaveAsync() > 0;
                else throw new Exception("Failed to send email for reset password!");
            }
            catch
            {
                //await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteUser(long id)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                var user = await userRepository.GetAll()
                    .AsNoTracking()
                    .Include(u => u.PublisherProfile)
                    .Include(u => u.AdvertiserProfile)
                    .Include(u => u.AffiliateSources)
                    .Include(u => u.Campaigns)
                    .Include(u => u.SubPurchases)
                    .FirstOrDefaultAsync(u => u.Id == id);
                if (user is not null)
                {
                    if (user.Status == UserStatus.Deleted && user.Role == UserRoles.Publisher)
                    {
                        if (user.AffiliateSources.Any())
                        {
                            var pubSrcRepository = _unitOfWork.GetRepository<PublisherSource>();
                            pubSrcRepository.DeleteRange(user.AffiliateSources);
                        }
                        if (user.PublisherProfile is not null)
                        {
                            var pubProfileRepository = _unitOfWork.GetRepository<PublisherProfile>();
                            pubProfileRepository.Delete(user.PublisherProfile);
                        }
                        //TODO: Manually delete other data of other tables to avoid FK conflict.
                        userRepository.Delete(user);
                        return await _unitOfWork.SaveAsync() > 0;
                    }
                    else if (user.Status == UserStatus.Deleted && user.Role == UserRoles.Advertiser)
                    {
                        if (user.AdvertiserProfile is not null)
                        {
                            var advProfileRepository = _unitOfWork.GetRepository<AdvertiserProfile>();
                            advProfileRepository.Delete(user.AdvertiserProfile);
                        }
                        if (user.Campaigns.Any())
                        {
                            var campaignRepository = _unitOfWork.GetRepository<Campaign>();
                            campaignRepository.DeleteRange(user.Campaigns);
                        }
                        if (user.SubPurchases.Any())
                        {
                            var subPurchaseRepository = _unitOfWork.GetRepository<SubPurchase>();
                            subPurchaseRepository.DeleteRange(user.SubPurchases);
                        }
                        //TODO: Manually delete other data of other tables to avoid FK conflict.
                        userRepository.Delete(user);
                        return await _unitOfWork.SaveAsync() > 0;
                    }
                    else
                    {
                        //TODO: Change the message and exception type
                        throw new Exception("Exception.");
                    }
                }
                else
                {
                    throw new KeyNotFoundException("User does not exist!");
                }
            }
            catch
            {
                //await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<PaginationResponse<UserResponse>> GetUsers(PaginationRequest request)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var users = await userRepository.GetAll()
                .AsNoTracking()
                .Where(u => u.Role == UserRoles.Publisher || u.Role == UserRoles.Advertiser)
                .Skip((request.pageNumber - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToListAsync();
            if (!users.Any())
                throw new NoDataRetrievalException("No data for users!");
            var totalCount = users.Count();

            var data = _mapper.Map<List<UserResponse>>(users);
            return new PaginationResponse<UserResponse>(data, totalCount, request.pageNumber, request.pageSize);
        }

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
                //var walletRepository = _unitOfWork.GetRepository<Wallet>();
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

                // Send email verification to user
                var verificationUrl = @$"{_appBaseUrl}/users/{user.Id}/verify-account";
                var msg = new EmailMessage
                {
                    To = user.Email,
                    Subject = "Account verification."
                };
                var verificationResult = await _emailService.SendVerificationEmail(msg, verificationUrl);
                if (verificationResult)
                    return await _unitOfWork.SaveAsync() > 0;
                else throw new Exception("Email sending failure!");
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
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

        public async Task<bool> UpdatePassword(string token, long userId, UpdatePasswordRequest request)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                if (request.NewPassword.Trim().ToLower() != request.ConfirmedPassword.Trim().ToLower())
                    throw new ArgumentException("Password does not match!");
                var user = await userRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == userId && u.ResetPasswordToken == token);
                if (user is null)
                    throw new KeyNotFoundException("User does not exist!");
                if (DateTime.UtcNow > user.ExpiryDate)
                    throw new Exception("Reset token is expired!");

                // Update the new password, reset the value of token and expiry time
                _ = _mapper.Map(request, user);
                userRepository.Update(user);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch
            {
                //await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
