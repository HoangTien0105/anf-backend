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
    public class PublisherService(IUnitOfWork unitOfWork, IMapper mapper) : IPublisherService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> AddAffiliateSources(long publisherId, List<AffiliateSourceCreateRequest> requests)
        {
            try
            {
                if (requests.Any())
                    throw new ArgumentException("Request data is invalid. Please check again!");
                var pubSourceRepository = _unitOfWork.GetRepository<PublisherSource>();
                var userRepository = _unitOfWork.GetRepository<User>();

                var publisher = await userRepository.FindByIdAsync(publisherId);
                if (publisher is null)
                {
                    throw new KeyNotFoundException("Publisher does not exist!");
                }
                // Mapping publisher's id into sources
                foreach (var publisherSource in requests)
                {
                    publisherSource.PublisherId = publisherId;
                }
                var sources = _mapper.Map<List<PublisherSource>>(requests);
                pubSourceRepository.AddRange(sources);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch
            {
                //await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> AddProfile(long publisherId, PublisherProfileRequest value)
        {
            try
            {
                if (value.PublisherId != publisherId)
                    throw new ArgumentException("Publisher's id is not match!");
                var userRepository = _unitOfWork.GetRepository<User>();
                var pubProfileRepository = _unitOfWork.GetRepository<PublisherProfile>();
                if (value is null) throw new NullReferenceException("Invalid request data. Please check again!");
                // Check whether a publisher is existed in the platform
                var publisher = await userRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == value.PublisherId && x.Status == Core.Enums.UserStatus.Active);
                if (publisher is null) throw new KeyNotFoundException("Publisher does not exist!");
                
                var duplicatedProfile = await pubProfileRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.PublisherId == value.PublisherId);
                if (duplicatedProfile is not null)
                {
                    throw new ArgumentException("Publisher's profile is existed!");
                }
                var profile = _mapper.Map<PublisherProfile>(value);
                pubProfileRepository.Add(profile);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch
            {
                //await _unitOfWork.RollbackAsync();
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
                pubSourceRepository.Delete(source);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch 
            {
                //await _unitOfWork.SaveAsync();
                throw;
            }
        }
        
        public async Task<bool> DeleteAffiliateSources(List<long> sourceIds)
        {
            try
            {
                var pubSourceRepository = _unitOfWork.GetRepository<PublisherSource>();
                foreach (var id in sourceIds)
                {
                    var affiliateSource = await pubSourceRepository.GetAll()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == id);
                    if (affiliateSource is null)
                        throw new KeyNotFoundException("Source does not exist!");
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

        public async Task<List<AffiliateSourceResponse>> GetAffiliateSourceOfPublisher(long publisherId)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var pubSrcRepository = _unitOfWork.GetRepository<PublisherSource>();

            var publisher = await userRepository.FindByIdAsync(publisherId);
            if (publisher is null)
                throw new KeyNotFoundException("Publisher does not exist!");
            
            var affiliateSources = await pubSrcRepository.GetAll()
                .AsNoTracking()
                .Where(p => p.PublisherId == publisherId && p.Status == AffSourceStatus.Verified)
                .ToListAsync();
            if (!affiliateSources.Any())
                throw new NoDataRetrievalException("No data of affiliate source!");
            var response = _mapper.Map<List<AffiliateSourceResponse>>(affiliateSources);
            return response;
        }

        public async Task<PublisherResponse> GetPublisherProfile(long publisherId)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var publisher = await userRepository.GetAll()
                .AsNoTracking()
                .Include(p => p.PublisherProfile)
                .Where(p => p.Role == Core.Enums.UserRoles.Publisher && p.Id == publisherId 
                    && p.Status == UserStatus.Active) 
                .FirstOrDefaultAsync();
            if (publisher is null)
                throw new KeyNotFoundException("Publisher does not exist!");
            var response = _mapper.Map<PublisherResponse>(publisher);
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
    }
}
