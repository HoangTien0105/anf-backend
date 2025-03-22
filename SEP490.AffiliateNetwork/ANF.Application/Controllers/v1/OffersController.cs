using ANF.Core.Commons;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    public class OffersController(IOfferService offerService) : BaseApiController
    {
        private readonly IOfferService _offerService = offerService;

        /// <summary>
        /// Get all offers
        /// </summary>
        /// <param name="request">Pagination request model</param>
        /// <returns></returns>
        [HttpGet("offers")]
        [Authorize]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOffers([FromQuery] PaginationRequest request)
        {
            var offers = await _offerService.GetOffers(request);
            return Ok(new ApiResponse<PaginationResponse<OfferResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = offers
            });
        }

        /// <summary>
        /// Get offer by id
        /// </summary>
        /// <param name="id">Offer id</param>
        /// <returns></returns>
        [HttpGet("offers/{id}")]
        [Authorize]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOffer(long id)
        {
            var offer = await _offerService.GetOffer(id);
            return Ok(new ApiResponse<OfferResponse>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = offer
            });
        }


        /// <summary>
        /// Create offers
        /// </summary>
        /// <param name="request">Offer data json</param>
        /// <returns></returns>
        [HttpPost("offers")]
        [Authorize(Roles = "Advertiser")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOffer([FromForm] OfferCreateRequest request)
        {

            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }
            var result = await _offerService.CreateOffer(request);
            if (!result) return BadRequest();

            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Create offer successfully"
            });
        }

        /// <summary>
        /// Update offer
        /// </summary>
        /// <param name="id">Offer id</param>
        /// <param name="request">Offer data</param>
        /// <returns></returns>
        [HttpPut("offers/{id}")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Advertiser, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOffer(long id, OfferUpdateRequest request)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }

            var result = await _offerService.UpdateOffer(id, request);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Update successfully"
            });
        }

        /// <summary>
        /// Delete offer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("offers/{id}")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Advertiser, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteOffer(long id)
        {
            var result = await _offerService.DeleteOffer(id);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Delete success."
            });
        }

        /// <summary>
        /// Apply offer for publishers
        /// </summary>
        /// <param name="pubId">Publisher code</param>
        /// <param name="offerId">Offer id</param>
        /// <returns></returns>
        [HttpPost("offers/publisher")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Publisher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApplyOffer(string pubId, long offerId)
        {
            var result = await _offerService.ApplyOffer(pubId, offerId);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Apply success."
            });
        }

        /// <summary>
        /// Update apply offer request for advertiser
        /// </summary>
        /// <param name="id">Publisher Offer Id</param>
        /// <param name="status">Request status</param>
        /// <param name="rejectReason">Reject reason</param>
        /// <returns></returns>
        [HttpPatch("offers/pubOffers/{id}/status")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePubOfferStatus(long id, string status, string? rejectReason)
        {
            var result = await _offerService.UpdateApplyOfferStatus(id, status, rejectReason);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Update success."
            });
        }

        /// <summary>
        /// Get publishers of an offer
        /// </summary>
        /// <param name="id">Offer's id</param>
        /// <returns></returns>
        [HttpGet("offers/{id}/publishers")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Advertiser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPublisherOfAnOffer(long id)
        {
            //TODO: Is pagination required for this endpoint?
            var publishers = await _offerService.GetPublisherOfOffer(id);
            return Ok(new ApiResponse<List<PublisherOfferResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = publishers
            });
        }
    }
}
