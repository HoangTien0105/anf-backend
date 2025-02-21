using ANF.Core.Models.Requests;

namespace ANF.Core.Services
{
    public interface IPublisherService
    {
        Task<bool> AddProfile(long publisherId, PublisherProfileRequest value);
    }
}
