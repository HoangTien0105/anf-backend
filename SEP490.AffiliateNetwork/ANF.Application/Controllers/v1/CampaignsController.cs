using Microsoft.AspNetCore.Mvc;
using ANF.Core.Services;
using Asp.Versioning;
using ANF.Core.Models.Responses;
using ANF.Core.Models.Requests;
using ANF.Core.Commons;
using Microsoft.AspNetCore.Authorization;

namespace ANF.Application.Controllers.v1
{
    public class CampaignsController(ICampaignService campaignService) : BaseApiController
    {
        private readonly ICampaignService _campaignService = campaignService;

        /// <summary>
        /// Get campaigns with verified status
        /// </summary>
        /// <param name="request">Pagination request model</param>
        /// <returns></returns>
        [HttpGet("campaigns")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCampaigns([FromQuery] PaginationRequest request)
        {
            var campaigns = await _campaignService.GetCampaigns(request);
            return Ok(new ApiResponse<PaginationResponse<CampaignDetailedResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = campaigns
            });
        }

        /// <summary>
        /// Get campaign for advertiser, admin
        /// </summary>
        /// <param name="id">Campaign's id</param>
        /// <returns></returns>
        [HttpGet("campaigns/{id}")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Advertiser, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCampaign(long id)
        {
            var campaign = await _campaignService.GetCampaign(id);
            return Ok(new ApiResponse<CampaignDetailedResponse>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = campaign
            });
        }

        /// <summary>
        /// Get campaign for publisher
        /// </summary>
        /// <param name="id">Campaign's id</param>
        /// <returns></returns>
        [HttpGet("campaigns/{id}/publishers")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Publisher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCampaignForPublisher(long id)
        {
            var campaign = await _campaignService.GetCampaignForPublisher(id);
            return Ok(new ApiResponse<CampaignPubDetailedResponse>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = campaign
            });
        }

        /// <summary>
        /// Get campaigns including offers for admin with all status
        /// </summary>
        /// <param name="request">Pagination request model</param>
        /// <returns></returns>
        [HttpGet("campaigns/offers")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCampaignsWithOffers([FromQuery] PaginationRequest request)
        {
            var campaigns = await _campaignService.GetCampaignsWithOffers(request);
            return Ok(new ApiResponse<PaginationResponse<CampaignResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = campaigns
            });
        }

        /// <summary>
        /// Get campaigns including offers for an advertiser with all status
        /// </summary>
        /// <param name="request">Pagination data</param>
        /// <param name="code">Advertiser's code</param>
        /// <returns></returns>
        [HttpGet("campaigns/advertisers/{code}/offers")] 
        [MapToApiVersion(1)]
        [Authorize(Roles = "Admin, Advertiser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCampaignsByAdvertiserWithOffers([FromQuery] PaginationRequest request, string code)
        {
            var campaigns = await _campaignService.GetCampaignsByAdvertisersWithOffers(request, code);
            return Ok(new ApiResponse<PaginationResponse<CampaignResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = campaigns
            });
        }

        /// <summary>
        /// Update campaign
        /// </summary>
        /// <param name="id">Campaign's id</param>
        /// <param name="request">Campaign update request</param>
        /// <returns></returns>
        [HttpPut("campaigns/{id}")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Admin, Advertiser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCampaign(long id, [FromForm] CampaignUpdateRequest request)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }

            var result = await _campaignService.UpdateCampaignInformation(id, request);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Update successfully"
            });
        }

        /// <summary>
        /// Update cammpaign status
        /// </summary>
        /// <param name="id">Campaign's id</param>
        /// <param name="request">Campaign status updated model</param>
        /// <returns></returns>
        [HttpPatch("campaigns/admin/{id}/status")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCampaignStatus(long id, [FromBody] CampaignOfferStatusUpdatedRequest request)
        {
            var result = await _campaignService.UpdateCampaignStatus(id, request.Status, request.RejectReason);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Update status successfully"
            });
        }

        /// <summary>
        /// Create campaigns
        /// </summary>
        /// <param name="request">Campaign data</param>
        /// <returns></returns>
        [HttpPost("campaigns")]
        [Authorize(Roles = "Advertiser")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCampaign([FromForm] CampaignCreateRequest request)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }
            var result = await _campaignService.CreateCampaign(request);
            if (!result) return BadRequest();

            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Create campaign successfully"
            });
        }

        /// <summary>
        /// Delete campaigns
        /// </summary>
        /// <param name="id">Campaign's id</param>
        /// <returns></returns>
        [HttpDelete("campaigns/{id}")]
        [Authorize(Roles = "Admin, Advertiser")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCampaign(long id)
        {
            var result = await _campaignService.DeleteCampaign(id);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Delete success."
            });
        }
    }
}
