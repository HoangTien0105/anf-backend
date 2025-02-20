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

        public async Task<bool> AddProfile(PublisherProfileRequest value)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                var pubProfileRepository = _unitOfWork.GetRepository<PublisherProfile>();
                if (value is null) throw new NullReferenceException("Invalid request data. Please check again!");
                var publisher = await userRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == value.PublisherId && x.Status == Core.Enums.UserStatus.Active);
                if (publisher is null) throw new KeyNotFoundException("Publisher does not exist!");

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
