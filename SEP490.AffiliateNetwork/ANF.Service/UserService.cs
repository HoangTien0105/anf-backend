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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace ANF.Service
{
    public class UserService(IUnitOfWork unitOfWork,
                             TokenService tokenService,
                             IMapper mapper, IEmailService emailService,
                             IUserClaimsService userClaimsService,
                             ILogger<UserService> logger, HttpClient httpClient,
                             IOptions<BankLookupSettings> options) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly TokenService _tokenService = tokenService;
        private readonly IMapper _mapper = mapper;
        private readonly IEmailService _emailService = emailService;
        private readonly IUserClaimsService _userClaimsService = userClaimsService;
        private readonly ILogger<UserService> _logger = logger;
        private readonly HttpClient _httpClient = httpClient;
        private readonly BankLookupSettings _options = options.Value;

        //TODO: Change the application host when deploying successfully!
        private readonly string _appBaseUrl = "http://localhost:5272/api/affiliate-network";

        // URL for account number lookup
        private readonly string _bankLookupUrl = "https://api.banklookup.net/api/bank/id-lookup-prod";

        public async Task<bool> ActivateWallet(string userCode)
        {
            try
            {
                var currentUserCode = _userClaimsService.GetClaim(ClaimConstants.NameId);
                if (userCode != currentUserCode)
                    throw new UnauthorizedAccessException("User's code does not match!");

                var walletRepository = _unitOfWork.GetRepository<Wallet>();
                var wallet = await walletRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserCode == userCode);
                if (wallet is null)
                {
                    throw new KeyNotFoundException("Wallet does not exist!");
                }
                if (wallet.IsActive)
                    throw new Exception("Wallet has already activated!");

                wallet.IsActive = true;
                walletRepository.Update(wallet);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

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
                await _unitOfWork.RollbackAsync();
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
                await _unitOfWork.RollbackAsync();
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
                    .FirstOrDefaultAsync(u => u.Id == id);
                if (user is not null)
                {
                    if (user.Status == UserStatus.Deleted && user.Role == UserRoles.Publisher)
                    {
                        if (user.AffiliateSources.Any())
                        {
                            var pubSrcRepository = _unitOfWork.GetRepository<TrafficSource>();
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
                        //TODO: Manually delete other data of other tables to avoid FK conflict. (Transaction is one of the table need to be checked)
                        userRepository.Delete(user);
                        return await _unitOfWork.SaveAsync() > 0;
                    }
                    else
                    {
                        throw new Exception("Cannot delete user!");
                    }
                }
                else
                {
                    throw new KeyNotFoundException("User does not exist!");
                }
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<DetailedUserResponse> GetUserInformation()
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                var userCode = _userClaimsService.GetClaim(ClaimConstants.NameId);
                if (string.IsNullOrEmpty(userCode))
                {
                    throw new UnauthorizedAccessException("Cannot retrieve claims from the token!");
                }
                var user = await userRepository.GetAll()
                    .AsNoTracking()
                    .Include(u => u.UserBanks)
                    .Include(u => u.AdvertiserProfile)
                    .Include(u => u.PublisherProfile)
                    .Include(u => u.Wallet)
                    .FirstOrDefaultAsync(u => u.UserCode == userCode);
                if (user is null)
                {
                    throw new KeyNotFoundException("User does not exist!");
                }

                if (user.Role == UserRoles.Advertiser)
                {
                    var response = new DetailedUserResponse()
                    {
                        Id = user.Id,
                        UserCode = user.UserCode,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        CitizenId = user.CitizenId,
                        Address = user.Address,
                        DateOfBirth = user.DateOfBirth,
                        Email = user.Email,
                        Role = user.Role.ToString(),
                        Balance = user.Wallet.Balance,
                        ImageUrl = user.AdvertiserProfile?.ImageUrl,
                        BankResponses = user.UserBanks?.Select(ub => new UserBankResponse
                        {
                            Id = (int)ub.Id,
                            BankingNo = ub.BankingNo,
                            BankingProvider = ub.BankingProvider,
                        }).ToList() ?? new List<UserBankResponse>(),
                        AdvertiserProfile = user.AdvertiserProfile is not null ? new AdvertiserProfileInfoResponse()
                        {
                            CompanyName = user.AdvertiserProfile.CompanyName,
                            Industry = user.AdvertiserProfile.Industry,
                            Bio = user.AdvertiserProfile.Bio,
                        } : null
                    };
                    return response;
                }
                else if (user.Role == UserRoles.Publisher)
                {
                    var response = new DetailedUserResponse()
                    {
                        Id = user.Id,
                        UserCode = user.UserCode,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        CitizenId = user.CitizenId,
                        Address = user.Address,
                        DateOfBirth = user.DateOfBirth,
                        Email = user.Email,
                        Role = user.Role.ToString(),
                        Balance = user.Wallet.Balance,
                        ImageUrl = user.PublisherProfile?.ImageUrl,
                        BankResponses = user.UserBanks.Select(ub => new UserBankResponse
                        {
                            Id = (int)ub.Id,
                            BankingNo = ub.BankingNo,
                            BankingProvider = ub.BankingProvider,
                        }).ToList() ?? new List<UserBankResponse>(),
                        PublisherProfile = user.PublisherProfile is not null ? new PublisherProfileInfoResponse()
                        {
                            Specialization = user.PublisherProfile.Specialization,
                            Bio = user.PublisherProfile.Bio,
                        } : null
                    };

                    return response;
                }
                else
                {
                    var response = new DetailedUserResponse()
                    {
                        Id = user.Id,
                        UserCode = user.UserCode,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        CitizenId = user.CitizenId,
                        Address = user.Address,
                        DateOfBirth = user.DateOfBirth,
                        Email = user.Email,
                        Role = user.Role.ToString(),
                    };
                    return response;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e.StackTrace);
                throw;
            }
        }

        public async Task<bool> RegisterAccount(AccountCreateRequest request)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                var walletRepository = _unitOfWork.GetRepository<Wallet>();
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
                {
                    await _unitOfWork.SaveAsync();  // Save user's information into database to store user's wallet info
                }
                else throw new Exception("Email sending failure!");
                //Enable user's wallet
                var wallet = new Wallet
                {
                    UserCode = user.UserCode,
                };
                walletRepository.Add(wallet);
                return await _unitOfWork.SaveAsync() > 0;   // Save user's wallet information
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
                await _unitOfWork.RollbackAsync();
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
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<DetailedUserResponse> Login(string email, string password)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var user = await userRepository.GetAll()
                .AsNoTracking()
                .Include(u => u.PublisherProfile)
                .Include(u => u.AdvertiserProfile)
                .Include(u => u.UserBanks)
                .Include(u => u.Wallet)
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password && u.Status == UserStatus.Active);
            if (user is null) throw new KeyNotFoundException("User does not exist.");
            if (user.Status == UserStatus.Deactive)
            {
                throw new UnauthorizedAccessException("User's account has been deactivated! Please contact to the IT support.");
            }
            var token = _tokenService.GenerateToken(user);

            if (user.Role == UserRoles.Advertiser)
            {
                var response = new DetailedUserResponse()
                {
                    Id = user.Id,
                    UserCode = user.UserCode,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    CitizenId = user.CitizenId,
                    Address = user.Address,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email,
                    Role = user.Role.ToString(),
                    Balance = user.Wallet.Balance,
                    ImageUrl = user.AdvertiserProfile?.ImageUrl,
                    AccessToken = token,
                    BankResponses = user.UserBanks?.Select(ub => new UserBankResponse
                    {
                        Id = (int)ub.Id,
                        BankingNo = ub.BankingNo,
                        BankingProvider = ub.BankingProvider,
                    }).ToList() ?? new List<UserBankResponse>(),
                    AdvertiserProfile = user.AdvertiserProfile is not null ? new AdvertiserProfileInfoResponse()
                    {
                        CompanyName = user.AdvertiserProfile.CompanyName,
                        Industry = user.AdvertiserProfile.Industry,
                        Bio = user.AdvertiserProfile.Bio,
                    } : null
                };
                return response;
            }
            else if (user.Role == UserRoles.Publisher)
            {
                var response = new DetailedUserResponse()
                {
                    Id = user.Id,
                    UserCode = user.UserCode,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    CitizenId = user.CitizenId,
                    Address = user.Address,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email,
                    Role = user.Role.ToString(),
                    Balance = user.Wallet.Balance,
                    ImageUrl = user.PublisherProfile?.ImageUrl,
                    AccessToken = token,
                    BankResponses = user.UserBanks.Select(ub => new UserBankResponse
                    {
                        Id = (int)ub.Id,
                        BankingNo = ub.BankingNo,
                        BankingProvider = ub.BankingProvider,
                    }).ToList() ?? new List<UserBankResponse>(),
                    PublisherProfile = user.PublisherProfile is not null ? new PublisherProfileInfoResponse()
                    {
                        Specialization = user.PublisherProfile.Specialization,
                        Bio = user.PublisherProfile.Bio,
                    } : null
                };

                return response;
            }
            else
            {
                // For admin
                var response = new DetailedUserResponse()
                {
                    Id = user.Id,
                    UserCode = user.UserCode,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    CitizenId = user.CitizenId,
                    Address = user.Address,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email,
                    Role = user.Role.ToString(),
                    AccessToken = token,
                };
                return response;
            }
        }

        public async Task<PaginationResponse<AdvertiserResponse>> GetAdvertisers(PaginationRequest request)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var users = await userRepository.GetAll()
                .AsNoTracking()
                .Where(u => u.Role == UserRoles.Advertiser)
                .Skip((request.pageNumber - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToListAsync();
            if (!users.Any())
                throw new NoDataRetrievalException("No data for users!");
            var totalCount = users.Count();

            var data = _mapper.Map<List<AdvertiserResponse>>(users);
            return new PaginationResponse<AdvertiserResponse>(data, totalCount, request.pageNumber, request.pageSize);
        }

        public async Task<PaginationResponse<PublisherResponse>> GetPublishers(PaginationRequest request)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var users = await userRepository.GetAll()
                .AsNoTracking()
                .Where(u => u.Role == UserRoles.Publisher)
                .Skip((request.pageNumber - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToListAsync();
            if (!users.Any())
                throw new NoDataRetrievalException("No data for users!");
            var totalCount = users.Count();

            var data = _mapper.Map<List<PublisherResponse>>(users);
            return new PaginationResponse<PublisherResponse>(data, totalCount, request.pageNumber, request.pageSize);
        }

        public async Task<bool> AddBankingInformation(List<UserBankCreateRequest> requests)
        {
            var result = false;
            try
            {
                var userBankRepository = _unitOfWork.GetRepository<UserBank>();
                var currentUserCode = _userClaimsService.GetClaim(ClaimConstants.NameId);
                if (string.IsNullOrEmpty(currentUserCode))
                    throw new UnauthorizedAccessException("Invalid user's token!");

                foreach (var request in requests)
                {
                    var isDuplicated = await userBankRepository.GetAll()
                        .AsNoTracking()
                        .AnyAsync(ub => ub.BankingNo == request.BankingNo);
                    if (isDuplicated)
                        throw new DuplicatedException("This banking number has already existed in the platform!");
                    
                    var userBank = new UserBank
                    {
                        UserCode = currentUserCode,
                        UserName = request.AccountName.ToUpper(),
                        BankingNo = request.BankingNo,
                        BankingProvider = request.BankingName,
                        AddedDate = DateTime.Now,
                    };
                    userBankRepository.Add(userBank);
                    result = await _unitOfWork.SaveAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message, ex.InnerException);
                await _unitOfWork.RollbackAsync();
                throw;
            }
            return result;
        }

        public async Task<bool> UpdateBankingInformation(long userBankId, UserBankUpdatedRequest request)
        {
            try
            {
                var currentUserCode = _userClaimsService.GetClaim(ClaimConstants.NameId);
                if (string.IsNullOrEmpty(currentUserCode))
                    throw new UnauthorizedAccessException("Invalid user's code!");

                var userBankRepository = _unitOfWork.GetRepository<UserBank>();
                var bankingInformation = await userBankRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.Id == userBankId) ??
                        throw new KeyNotFoundException("User's banking information does not exist!");

                var payload = new
                {
                    Bank = request.BankingCode,
                    Account = request.BankingNo
                };
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, _bankLookupUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
                };
                httpRequest.Headers.Add("x-api-key", _options.ApiKey);
                httpRequest.Headers.Add("x-api-secret", _options.ApiSecret);

                var response = await _httpClient.SendAsync(httpRequest);
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Failed to verify bank account {request.BankingNo}");

                bankingInformation.UserName = request.AccountName.ToUpper();
                bankingInformation.BankingNo = request.BankingNo;
                bankingInformation.BankingProvider = request.BankingName;
                userBankRepository.Update(bankingInformation);

                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e.Message, e.StackTrace);
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<string> LookupBankAccount(string bankCode, string accountNo)
        {
            try
            {
                var payload = new
                {
                    bank = bankCode,
                    account = accountNo
                };
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, _bankLookupUrl)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
                };
                httpRequest.Headers.Add("x-api-key", _options.ApiKey);
                httpRequest.Headers.Add("x-api-secret", _options.ApiSecret);

                var response = await _httpClient.SendAsync(httpRequest);
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Failed to verify account number: {accountNo}");

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BankLookupResponse>(content);

                return result?.Data?.OwnerName ?? string.Empty;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e.StackTrace);
                throw;
            }
        }
    }
}
