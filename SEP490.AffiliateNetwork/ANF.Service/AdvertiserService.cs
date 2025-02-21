using ANF.Core;
using ANF.Core.Models.Entities;
using ANF.Core.Models.Requests;
using ANF.Core.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ANF.Service
{
    public class AdvertiserService(IUnitOfWork unitOfWork, IMapper mapper) : IAdvertiserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> AddProfile(long advertiserId, AdvertiserProfileRequest profile)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                var advProfileRepository = _unitOfWork.GetRepository<AdvertiserProfile>();
                if (profile.AdvertiserId != advertiserId)
                    throw new ArgumentException("Publisher's id is not match!");
                if (profile is null) 
                    throw new NullReferenceException("Invalid request data. Please check again!");
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
                var mappedProfile = _mapper.Map<AdvertiserProfile>(profile);
                advProfileRepository.Add(mappedProfile);
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
