using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;

namespace ANF.Core.Services
{
    public interface IPublisherService
    {
        Task<bool> AddProfile(long publisherId, PublisherProfileRequest value);

        Task<PublisherResponse> GetPublisherProfile(long publisherId);
    }
}
