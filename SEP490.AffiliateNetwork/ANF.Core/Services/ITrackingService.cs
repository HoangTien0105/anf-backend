using ANF.Core.Commons;
using Microsoft.AspNetCore.Http;

namespace ANF.Core.Services
{
    public interface ITrackingService
    {
        public Task<string> StoreParams(long offerId, string publisherCode, string? siteId, HttpRequest httpRequest);

        Task ProcessTrackingEvent(TrackingConversionEvent trackingConversionEvent);
    }
}
