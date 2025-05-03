using ANF.Core.Commons;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ANF.Application.Controllers.v1
{
    public class AdminStatsController(IUserService userService) : BaseApiController
    {
        private readonly IUserService _userService = userService;

        /// <summary>
        /// User statistics for admin
        /// </summary>
        /// <param name="from">From date</param>
        /// <param name="to">To date</param>
        /// <returns></returns>
        [HttpGet("stats/users")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<UserStatsAdminResponse>>> GetUserStats(
            [Required] DateTime from, 
            [Required] DateTime to
        )
        {
            var response = await _userService.GetUserStats(from, to);
            return Ok(new ApiResponse<UserStatsAdminResponse>
            {
                IsSuccess = true,
                Message = "Success!",
                Value = response
            });
        }

        /// <summary>
        /// Campaign statistics for admin
        /// </summary>
        /// <param name="from">From date</param>
        /// <param name="to">To date</param>
        /// <returns></returns>
        [HttpGet("stats/campaigns")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<CampaignStatsAdminResponse>>> GetCampaignStats(
            [Required] DateTime from,
            [Required] DateTime to
        )
        {
            var result = await _userService.GetCampaignStats(from, to);
            return Ok(new ApiResponse<CampaignStatsAdminResponse>
            {
                IsSuccess = true,
                Message = "Success!",
                Value = result
            });
        }

        /// <summary>
        /// Ticket statistics for admin
        /// </summary>
        /// <param name="from">From date</param>
        /// <param name="to">To date</param>
        /// <returns></returns>
        [HttpGet("stats/complaint-tickets")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<TicketStatsAdminResponse>>> GetTicketStats(
            [Required] DateTime from,
            [Required] DateTime to
        )
        {
            var result = await _userService.GetTicketStats(from, to);
            return Ok(new ApiResponse<TicketStatsAdminResponse>
            {
                IsSuccess = true,
                Message = "Success!",
                Value = result
            });
        }
    }
}
