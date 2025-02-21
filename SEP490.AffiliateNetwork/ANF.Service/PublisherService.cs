using ANF.Core;
using ANF.Core.Models.Entities;
using ANF.Core.Models.Requests;
using ANF.Core.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ANF.Service
{
    public class PublisherService(IUnitOfWork unitOfWork, IMapper mapper) : IPublisherService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        
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
    }
}
