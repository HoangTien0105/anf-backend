using Microsoft.AspNetCore.Http;

namespace ANF.Core.Services
{
    public interface ITrackingService
    {
        public Task<string> StoreParams(long offerId, string publisherCode, string? siteId, HttpRequest httpRequest);
    }
}
