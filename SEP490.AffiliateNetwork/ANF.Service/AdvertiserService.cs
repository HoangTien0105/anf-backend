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

namespace ANF.Service
{
    public class AdvertiserService(IUnitOfWork unitOfWork, IMapper mapper,
        ICloudinaryService cloudinaryService,
        IUserClaimsService userClaimsService) : IAdvertiserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        private readonly IUserClaimsService _userClaimsService = userClaimsService;

        public async Task<bool> AddBankingInformation(string advertiserCode, List<UserBankCreateRequest> requests)
        {
            try
            {
                var currentAdvertiserCode = _userClaimsService.GetClaim(ClaimConstants.NameId);
                if (advertiserCode != currentAdvertiserCode)
                    throw new UnauthorizedAccessException("Advertiser's code does not match!");
                var userBankRepository = _unitOfWork.GetRepository<UserBank>();
                if (!requests.Any())
                    throw new ArgumentException("Invalid requested data!");
                foreach (var item in requests)
                {
                    var isDuplicate = await userBankRepository.GetAll()
                        .AsNoTracking()
                        .AnyAsync(ub => ub.UserCode == advertiserCode && ub.BankingNo == item.BankingNo);
                    if (isDuplicate) throw new DuplicatedException("This banking number has already existed!");
                }
                var banks = _mapper.Map<List<UserBank>>(requests, opt => opt.Items["UserCode"] = advertiserCode);
                userBankRepository.AddRange(banks);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> AddProfile(long advertiserId, AdvertiserProfileRequest profile)
        {
            try
            {
                var currentAdvertiserId = _userClaimsService.GetClaim(ClaimConstants.Primarysid);
                if (advertiserId != long.Parse(currentAdvertiserId))
                    throw new UnauthorizedAccessException("Advertiser's id does not match!");

                var userRepository = _unitOfWork.GetRepository<User>();
                var advProfileRepository = _unitOfWork.GetRepository<AdvertiserProfile>();
                var imageUrl = string.Empty;

                if (profile.AdvertiserId != advertiserId)
                    throw new ArgumentException("Advertiser's id is not match!");
                if (profile is null)
                    throw new NullReferenceException("Invalid request data. Please check again!");
                if (profile.Image is not null)
                    imageUrl = await _cloudinaryService.UploadImageAsync(profile.Image);
                // Check whether an advertiser is existed in platform
                var advertiser = await userRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == profile.AdvertiserId && x.Status == Core.Enums.UserStatus.Active);
                if (advertiser is null) throw new KeyNotFoundException("Advertiser does not exist!");

                // Check whether the profile is existed in platform
                var duplicatedProfile = await advProfileRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.AdvertiserId == profile.AdvertiserId);
                if (duplicatedProfile is not null)
                {
                    throw new ArgumentException("Advertiser's profile is existed!");
                }
                var mappedProfile = _mapper.Map<AdvertiserProfile>(profile, opt => opt.Items["ImageUrl"] = imageUrl);
                advProfileRepository.Add(mappedProfile);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteBankingInformation(List<long> ubIds)
        {
            try
            {
                var userBankRepository = _unitOfWork.GetRepository<UserBank>();
                var isFound = false;
                foreach (var item in ubIds)
                {
                    var bankingInfo = await userBankRepository.FindByIdAsync(item);
                    if (bankingInfo is not null)
                    {
                        userBankRepository.Delete(bankingInfo);
                        isFound = true;
                    }
                }
                if (!isFound)
                    throw new KeyNotFoundException("Banking information does not exist!");
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<AdvertiserProfileResponse> GetAdvertiserProfile(long advertiserId)
        {
            var currentAdvertiserId = _userClaimsService.GetClaim(ClaimConstants.Primarysid);
            if (advertiserId != long.Parse(currentAdvertiserId))
                throw new UnauthorizedAccessException("Advertiser's id does not match!");

            var userRepository = _unitOfWork.GetRepository<User>();
            var advertiser = await userRepository.GetAll()
                .AsNoTracking()
                .Include(u => u.AdvertiserProfile)
                .Include(u => u.UserBanks)
                .FirstOrDefaultAsync(ad => ad.Id == advertiserId && ad.Role == Core.Enums.UserRoles.Advertiser);
            if (advertiser is null)
                throw new KeyNotFoundException("Advertiser does not exist!");
            var response = _mapper.Map<AdvertiserProfileResponse>(advertiser);
            return response;
        }

        public async Task<List<AffiliateSourceResponse>> GetTrafficSourceOfPublisher(long publisherId)
        {
            var currentAdvertiserCode = _userClaimsService.GetClaim(ClaimConstants.NameId);
            if (string.IsNullOrEmpty(currentAdvertiserCode))
                throw new UnauthorizedAccessException("Advertiser's code is empty!");

            var trafficSourceRepository = _unitOfWork.GetRepository<TrafficSource>();
            var sources = await trafficSourceRepository.GetAll()
                .AsNoTracking()
                .Where(x => x.PublisherId == publisherId && x.Status == TrackingSourceStatus.Verified)
                .ToListAsync();
            if (!sources.Any())
                throw new NoDataRetrievalException("No data of traffic sources!");

            return _mapper.Map<List<AffiliateSourceResponse>>(sources);
        }

        public async Task<bool> UpdateBankingInformation(long userBankId, UserBankUpdateRequest request)
        {
            try
            {
                var userBankRepository = _unitOfWork.GetRepository<UserBank>();
                if (request is null)
                    throw new ArgumentException("Invalid requested data!");
                var bank = await userBankRepository.FindByIdAsync(userBankId);
                if (bank is null)
                    throw new KeyNotFoundException("Banking information does not exist!");
                if (bank.BankingNo == request.BankingNo && bank.BankingProvider == request.BankingProvider)
                    throw new DuplicatedException("No changes detected. Banking information is identical!");
                _ = _mapper.Map(request, bank);
                userBankRepository.Update(bank);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateProfile(long advertiserId, AdvertiserProfileUpdatedRequest request)
        {
            try
            {
                var currentAdvertiserId = _userClaimsService.GetClaim(ClaimConstants.Primarysid);
                if (advertiserId != long.Parse(currentAdvertiserId))
                    throw new UnauthorizedAccessException("Advertiser's id does not match!");

                var userRepository = _unitOfWork.GetRepository<User>();
                var advProfileRepository = _unitOfWork.GetRepository<AdvertiserProfile>();
                var imageUrl = string.Empty;

                if (request is null)
                    throw new ArgumentException("Invalid requested data!");
                if (request.Image is not null)
                    imageUrl = await _cloudinaryService.UploadImageAsync(request.Image);

                var advertiser = await userRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == advertiserId);
                var profile = await advProfileRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.AdvertiserId == advertiserId);

                if (advertiser is null)
                    throw new KeyNotFoundException("Advertiser does not exist!");
                if (profile is null)
                    throw new KeyNotFoundException("Advertiser's profile does not exist!");

                _ = _mapper.Map(request, advertiser);
                _ = _mapper.Map(request, profile, opts: opt => opt.Items["ImageUrl"] = imageUrl);
                userRepository.Update(advertiser);
                advProfileRepository.Update(profile);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
