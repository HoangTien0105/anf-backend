using ANF.Core.Models.Requests;

namespace ANF.Core.Services
{
    public interface IAdvertiserService
    {
        Task<bool> AddProfile(long advertiserId, AdvertiserProfileRequest profile);
    }
}
