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
    public class PublisherService(IUnitOfWork unitOfWork, IMapper mapper,
        ICloudinaryService cloudinaryService) : IPublisherService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;

        public async Task<bool> AddAffiliateSources(long publisherId, List<AffiliateSourceCreateRequest> requests)
        {
            try
            {
                if (!requests.Any())
                    throw new ArgumentException("Request data is invalid. Please check again!");
                var pubSourceRepository = _unitOfWork.GetRepository<PublisherSource>();
                var userRepository = _unitOfWork.GetRepository<User>();

                var publisher = await userRepository.FindByIdAsync(publisherId);
                if (publisher is null)
                {
                    throw new KeyNotFoundException("Publisher does not exist!");
                }
                // Pass publisherId to AutoMapper via Items dictionary
                var sources = _mapper.Map<List<PublisherSource>>(requests, opts => opts.Items["PublisherId"] = publisherId);
                pubSourceRepository.AddRange(sources);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> AddBankingInformation(Guid publisherCode, List<UserBankCreateRequest> requests)
        {
            try
            {
                var userBankRepository = _unitOfWork.GetRepository<UserBank>();
                if (!requests.Any())
                    throw new ArgumentException("Invalid requested data!");
                foreach (var item in requests)
                {
                    var isDuplicate = await userBankRepository.GetAll()
                        .AsNoTracking()
                        .AnyAsync(ub => ub.UserCode == publisherCode && ub.BankingNo == item.BankingNo);
                    if (isDuplicate) throw new DuplicatedException("This banking number has already existed!");
                }
                var banks = _mapper.Map<List<UserBank>>(requests, opt => opt.Items["UserCode"] = publisherCode);
                userBankRepository.AddRange(banks);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

        }

        public async Task<bool> AddProfile(long publisherId, PublisherProfileCreatedRequest value)
        {
            try
            {
                if (value.PublisherId != publisherId)
                    throw new ArgumentException("Publisher's id is not match!");
                var userRepository = _unitOfWork.GetRepository<User>();
                var pubProfileRepository = _unitOfWork.GetRepository<PublisherProfile>();
                var imageUrl = string.Empty;

                if (value is null) throw new NullReferenceException("Invalid request data. Please check again!");
                if (value.Image is not null)
                    imageUrl = await _cloudinaryService.UploadImageAsync(value.Image);
                // Check whether a publisher is existed in the platform
                var publisher = await userRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == value.PublisherId && x.Status == UserStatus.Active);
                if (publisher is null) throw new KeyNotFoundException("Publisher does not exist!");

                var duplicatedProfile = await pubProfileRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.PublisherId == value.PublisherId);
                if (duplicatedProfile is not null)
                {
                    throw new ArgumentException("Publisher's profile is existed!");
                }
                var profile = _mapper.Map<PublisherProfile>(value, opt => opt.Items["ImageUrl"] = imageUrl);
                pubProfileRepository.Add(profile);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteAffiliateSource(long sourceId)
        {
            try
            {
                var pubSourceRepository = _unitOfWork.GetRepository<PublisherSource>();
                var source = await pubSourceRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Id == sourceId);
                if (source is null)
                    throw new KeyNotFoundException("Source does not exist!");
                if (source.Status == AffSourceStatus.Verified)
                    throw new ArgumentException("The source cannot update because it's verified in system!");
                pubSourceRepository.Delete(source);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteAffiliateSources(List<long> sourceIds)
        {
            try
            {
                if (!sourceIds.Any())
                    throw new ArgumentException("Request data is invalid. Please check again!");
                var pubSourceRepository = _unitOfWork.GetRepository<PublisherSource>();
                foreach (var id in sourceIds)
                {
                    var affiliateSource = await pubSourceRepository.GetAll()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == id);
                    if (affiliateSource is null)
                        throw new KeyNotFoundException("Source does not exist!");
                    if (affiliateSource.Status == AffSourceStatus.Verified)
                        throw new ArgumentException("The source cannot update because it's verified in system!");
                    pubSourceRepository.Delete(affiliateSource);
                }
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch
            {
                //await _unitOfWork.RollbackAsync();
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

        public async Task<List<AffiliateSourceResponse>> GetAffiliateSourceOfPublisher(long publisherId)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var pubSrcRepository = _unitOfWork.GetRepository<PublisherSource>();

            var publisher = await userRepository.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == publisherId);
            if (publisher is null)
                throw new KeyNotFoundException("Publisher does not exist!");

            var affiliateSources = await pubSrcRepository.GetAll()
                .AsNoTracking()
                .Where(p => p.PublisherId == publisherId)
                .ToListAsync();
            if (!affiliateSources.Any())
                throw new NoDataRetrievalException("No data of affiliate source!");
            var response = _mapper.Map<List<AffiliateSourceResponse>>(affiliateSources);
            return response;
        }

        public async Task<PublisherProfileResponse> GetPublisherProfile(long publisherId)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var publisher = await userRepository.GetAll()
                .AsNoTracking()
                .Include(p => p.PublisherProfile)
                .Include(p => p.UserBanks)
                .Where(p => p.Role == UserRoles.Publisher && p.Id == publisherId
                    && p.Status == UserStatus.Active)
                .FirstOrDefaultAsync();
            if (publisher is null)
                throw new KeyNotFoundException("Publisher does not exist!");
            var response = _mapper.Map<PublisherProfileResponse>(publisher);
            return response;
        }

        public async Task<bool> UpdateAffiliateSource(long sourceId, AffiliateSourceUpdateRequest request)
        {
            try
            {
                var pubSourceRepository = _unitOfWork.GetRepository<PublisherSource>();
                if (request is null)
                    throw new ArgumentException("Invalid data request. Please check again!");
                var source = await pubSourceRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == sourceId);
                if (source is null)
                    throw new KeyNotFoundException("Source does not exist!");
                if (source.Status == AffSourceStatus.Verified)
                    throw new ArgumentException("The source cannot update because it's verified in system!");  //TODO: Change the suitable exception to handle this error.
                _ = _mapper.Map(request, source);

                pubSourceRepository.Update(source);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch
            {
                //await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateAffiliateSourceState(List<long> sIds)
        {
            try
            {
                var pubSourceRepository = _unitOfWork.GetRepository<PublisherSource>();
                foreach (var item in sIds)
                {
                    var source = await pubSourceRepository.FindByIdAsync(item);
                    if (source is null)
                        throw new KeyNotFoundException("Source does not exist!");
                    source.Status = AffSourceStatus.Verified;
                    pubSourceRepository.Update(source);
                }
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
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

        public async Task<bool> UpdateProfile(long publisherId, PublisherProfileUpdatedRequest request)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                var pubProfileRepository = _unitOfWork.GetRepository<PublisherProfile>();
                var imageUrl = string.Empty;

                if (request is null)
                    throw new ArgumentException("Invalid requested data!");
                if (request.Image != null)
                    imageUrl = await _cloudinaryService.UploadImageAsync(request.Image);

                var publisher = await userRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == publisherId);

                var profile = await pubProfileRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.PublisherId == publisherId);
                if (publisher is null || profile is null)
                    throw new KeyNotFoundException("Publisher does not exist!");

                _ = _mapper.Map(request, publisher);
                _ = _mapper.Map(request, profile, opt => opt.Items["ImageUrl"] = imageUrl);

                userRepository.Update(publisher);
                pubProfileRepository.Update(profile);
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
