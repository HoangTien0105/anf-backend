using ANF.Core.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    public class TrackingController(ITrackingService trackingService) : BaseApiController
    {
        private readonly ITrackingService _trackingService = trackingService;

        /// <summary>
        /// Store tracking data and redirect
        /// </summary>
        /// <param name="offerId"></param>
        /// <param name="publisherId"></param>
        /// <returns></returns>
        [HttpGet("tracking")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Store(long offerId, long publisherId)
        {
            try
            {
                string redirectUrl = await _trackingService.StoreParams(offerId, publisherId, HttpContext.Request);
                return Redirect(redirectUrl);
            }
            catch
            {
                throw;
            }
        }
    }
}