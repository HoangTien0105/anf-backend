using ANF.Core.Commons;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using ANF.Service;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ANF.Application.Controllers.v1
{
    public class PublisherStatsController(IPublisherStatsService publisherStatsService) : BaseApiController
    {
        private readonly IPublisherStatsService _publisherStatsService = publisherStatsService;


        /// <summary>
        /// Get publisher campaign's revenue statistics by campaign's id
        /// </summary>
        /// <param name="id">campaign's id</param>
        /// <param name="from">from date</param>
        /// <param name="to">to date</param>
        /// <returns></returns>
        [HttpGet("publisher-stats/campaign/{id}/revenue")]
        [Authorize(Roles = "Publisher, Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPublisherCampaignRevenueStatsByCampaignId(long id,
            [Required] DateTime from,
            [Required] DateTime to)
        {
            var publisherOfferStats = await _publisherStatsService.GetRevenueStatsByCampaignId(id, from, to);
            return Ok(new ApiResponse<List<PublisherStatsResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = publisherOfferStats
            });
        }

        /// <summary>
        /// Get publisher campaign's revenue statistics
        /// </summary>
        /// <param name="from">from date</param>
        /// <param name="to">to date</param>
        /// <returns></returns>
        [HttpGet("publisher/stats/revenue")]
        [Authorize(Roles = "Publisher, Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPublisherCampaignRevenueStats(
            [Required] DateTime from,
            [Required] DateTime to)
        {
            var publisherOfferStats = await _publisherStatsService.GetRevenueStats(from, to);
            return Ok(new ApiResponse<List<PublisherStatsResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = publisherOfferStats
            });
        }

        [HttpPost("publisher/stats")]
        [Authorize(Roles = "Publisher, Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GenerateStatsByCampaignId(
           [Required] DateTime from,
           [Required] DateTime to)
        {
            var publisherOfferStats = await _publisherStatsService.GeneratePublisherOfferStatsByPublisherCode(from, to);
            return Ok(new ApiResponse<bool>
            {
                IsSuccess = true,
                Message = "Success.",
            });
        }

    }
}
