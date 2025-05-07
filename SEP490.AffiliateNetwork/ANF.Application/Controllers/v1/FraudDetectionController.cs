using ANF.Core.Commons;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ANF.Application.Controllers.v1
{
    public class FraudDetectionController(IFraudDetectionService fraudDetectionService) : BaseApiController
    {
        private readonly IFraudDetectionService _fraudDetectionService = fraudDetectionService;

        /// <summary>
        /// Get fraud detection
        /// </summary>
        /// <param name="request">Pagination request model</param>
        /// <param name="dateRange">Date range request</param>
        /// <returns></returns>
        [HttpGet("frauds")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Advertiser, Publisher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFraudsDetection([FromQuery] PaginationRequest request,
            [FromQuery][Required] DateRangeRequest dateRange)
        {
            var frauds = await _fraudDetectionService.GetFraudDetections(request, dateRange.From, dateRange.To);
            return Ok(new ApiResponse<PaginationResponse<FraudDetectionResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = frauds
            });
        }
    }
}
