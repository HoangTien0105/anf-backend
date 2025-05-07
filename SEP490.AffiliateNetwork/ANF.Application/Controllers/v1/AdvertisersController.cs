using ANF.Core.Commons;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    public class AdvertisersController(IAdvertiserService advertiserService) : BaseApiController
    {
        private readonly IAdvertiserService _advertiserService = advertiserService;

        /// <summary>
        /// Get advertiser information
        /// </summary>
        /// <param name="id">Advertiser's id</param>
        /// <returns></returns>
        [HttpGet("advertisers/{id}/profile")]
        [Authorize(Roles = "Advertiser, Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAdvertiserInfo(long id)
        {
            var advertiser = await _advertiserService.GetAdvertiserProfile(id);
            return Ok(new ApiResponse<AdvertiserProfileResponse>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = advertiser
            });
        }

        /// <summary>
        /// Get verified publisher's traffic sources
        /// </summary>
        /// <param name="id">Publisher's id</param>
        /// <returns></returns>
        [HttpGet("publishers/{id}/available-traffic-sources")]
        [Authorize(Roles = "Advertiser")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTrafficSources(long id)
        {
            //TODO: Is pagination required for endpoint?
            var sources = await _advertiserService.GetTrafficSourceOfPublisher(id);
            return Ok(new ApiResponse<List<AffiliateSourceResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = sources
            });
        }

        /// <summary>
        /// Update advertiser's profile
        /// </summary>
        /// <param name="id">Advertiser's id</param>
        /// <param name="request">Requested data for updating</param>
        /// <returns></returns>
        [HttpPut("advertiser/{id}/profile")]
        [Authorize(Roles = "Advertiser")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAdvertiserProfile(long id, AdvertiserProfileUpdatedRequest request)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }
            var result = await _advertiserService.UpdateProfile(id, request);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Update advertiser's profile successfully!"
            });
        }

        /// <summary>
        /// Add profile of an advertiser
        /// </summary>
        /// <param name="id">Advertiser's id</param>
        /// <param name="value">Profile data</param>
        /// <returns></returns>
        [HttpPost("advertiser/{id}/profile")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Advertiser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddProfile(long id, [FromForm] AdvertiserProfileRequest value)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }
            var result = await _advertiserService.AddProfile(id, value);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Success"
            });
        }

        /// <summary>
        /// View the list of publishers waiting to be approved for an offer
        /// </summary>
        /// <param name="offerId">Offer's id</param>
        /// <returns>Publishers with detailed information</returns>
        [HttpGet("offers/{offerId}/pending-publishers")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Advertiser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ViewPublisherProfile(string offerId)
        {
            var result = await _advertiserService.GetPendingPublisherInOffer(offerId);
            return Ok(new ApiResponse<List<PublisherInformationForAdvertiser>>
            {
                IsSuccess = true,
                Message = "Success",
                Value = result
            });
        }


        [HttpGet("campaigns/{id}/click-stats")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Advertiser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AdvertiserCampaignStatsResponse>>> GetClickStats(long id,
             DateTime from,
             DateTime to)
        {
            var result = await _advertiserService.GetClickStatistics(id, from, to);
            return Ok(new ApiResponse<List<AdvertiserCampaignStatsResponse>>
            {
                IsSuccess = true,
                Message = "Success",
                Value = result
            });
        }

        [HttpGet("campaigns/click-stats")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Advertiser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AdvertiserCampaignStatsResponse>>> GetClickStats(
             DateTime from,
             DateTime to)
        {
            var result = await _advertiserService.GetClickStatisticsOfCampagins(from, to);
            return Ok(new ApiResponse<List<AdvertiserCampaignStatsResponse>>
            {
                IsSuccess = true,
                Message = "Success",
                Value = result
            });
        }

        [HttpGet("campaigns/{id}/device-stats")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Advertiser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AdvertiserCampaignStatsResponse>>> GetDeviceStats(long id,
            DateTime from,
            DateTime to)
        {
            var result = await _advertiserService.GetDeviceStatistics(id, from, to);
            return Ok(new ApiResponse<List<AdvertiserCampaignStatsResponse>>
            {
                IsSuccess = true,
                Message = "Success",
                Value = result
            });
        }
        

        [HttpGet("campaigns/{id}/offer-stats")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Advertiser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AdvertiserCampaignStatsResponse>>> GetOfferStats(long id,
            DateTime from,
            DateTime to)
        {
            var result = await _advertiserService.GetOfferStatistics(id, from, to);
            return Ok(new ApiResponse<List<AdvertiserCampaignStatsResponse>>
            {
                IsSuccess = true,
                Message = "Success",
                Value = result
            });
        }
    }
}
