using ANF.Core.Commons;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    public class StatisticsController : Controller
    {
        private readonly IStatisticService _statisticService;

        public StatisticsController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        /// <summary>
        /// Get advertiser offer statistics by offer's id
        /// </summary>
        /// <param name="id">offer's id</param>
        /// <returns></returns>
        [HttpGet("statistic/advertiser/offer/{id}")]
        [Authorize(Roles = "Advertiser, Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAdvertiserOfferStatsByOfferId(long id)
        {
            var advertiserOfferStats = await _statisticService.GetAdvertiserOfferStatsByOfferId(id);
            return Ok(new ApiResponse<AdvertiserOfferStatsResponse>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = advertiserOfferStats
            });
        }
        /// <summary>
        /// Get advertiser offer statistics by advertiser's code
        /// </summary>
        /// <param name="code">advertiser's code</param>
        /// <returns></returns>
        [HttpGet("statistic/advertiser/{code}/offer")]
        [Authorize(Roles = "Advertiser, Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAdvertiserOfferStatsByAdvertiserCode(string code)
        {
            var advertiserOfferStatsList = await _statisticService.GetAllAdvertiserOffersStatsByAdvertiserCode(code);
            return Ok(new ApiResponse<List<AdvertiserOfferStatsResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = advertiserOfferStatsList
            });
        }
        /// <summary>
        /// generate advertiser offer statistics by offer's id
        /// </summary>
        /// <param name="id">offer's id</param>
        /// <returns></returns>
        [HttpPost("statistic/advertiser/offer/{id}")]
        [Authorize(Roles = "Advertiser, Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GenerateAdvertiserOfferStatsByOfferId(long id)
        {
            var result = await _statisticService.GenerateAdvertiserOfferStatsByOfferId(id);
            if (result)
            {
                return Ok(new ApiResponse<string>
                {
                    IsSuccess = true,
                    Message = "Success.",
                });
            } else
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Generate advertiser offer statistics by advertiser's code
        /// </summary>
        /// <param name="code">advertiser's code</param>
        /// <returns></returns>
        [HttpPost("statistic/advertiser/{code}/offer")]
        [Authorize(Roles = "Advertiser, Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GenerateAdvertiserOfferStatsByAdvertiserCode(string code)
        {
            var result = await _statisticService.GenerateAdvertiserOfferStatsByAdvertiserCode(code);
            if (result)
            {
                return Ok(new ApiResponse<string>
                {
                    IsSuccess = true,
                    Message = "Success.",
                });
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get publisher offer statistics by offer's id
        /// </summary>
        /// <param name="id">offer's id</param>
        /// <param name="code">publisher's code</param>
        /// <returns></returns>
        [HttpGet("statistic/publisher/{code}/offer/{id}")]
        [Authorize(Roles = "Publisher, Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPublisherOfferStatsByOfferId(string code, long id)
        {
            var publisherOfferStats = await _statisticService.GetPublisherOfferStatsByOfferId(id, code);
            return Ok(new ApiResponse<PublisherOfferStatsResponse>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = publisherOfferStats
            });
        }
        /// <summary>
        /// Get publisher offer statistics by publisher's code
        /// </summary>
        /// <param name="code">publisher's code</param>
        /// <returns></returns>
        [HttpGet("statistic/publisher/{code}/offer")]
        [Authorize(Roles = "Publisher, Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPublisherOfferStatsByPublisherCode(string code)
        {
            var publisherOfferStatsList = await _statisticService.GetAllPublisherOfferStatsByPublisherCode(code);
            return Ok(new ApiResponse<List<PublisherOfferStatsResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = publisherOfferStatsList
            });
        }
        /// <summary>
        /// Generate publisher offer statistics by offer's id
        /// </summary>
        /// <param name="id">offer's id</param>
        /// <param name="code">publisher's code</param>
        /// <returns></returns>
        [HttpPost("statistic/publisher/{code}/offer/{id}")]
        [Authorize(Roles = "Publisher, Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GeneratePublisherOfferStatsByOfferId(string code, long id)
        {
            var result = await _statisticService.GeneratePublisherOfferStatsByOfferId(id, code);
            if (result)
            {
                return Ok(new ApiResponse<string>
                {
                    IsSuccess = true,
                    Message = "Success.",
                });
            }
            else
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Get publisher offer statistics by publisher's code
        /// </summary>
        /// <param name="code">publisher's code</param>
        /// <returns></returns>
        [HttpPost("statistic/publisher/{code}/offer")]
        [Authorize(Roles = "Publisher, Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GeneratePublisherOfferStatsByPublisherCode(string code)
        {
            var result = await _statisticService.GeneratePublisherOfferStatsByPublisherCode(code);
            if (result)
            {
                return Ok(new ApiResponse<string>
                {
                    IsSuccess = true,
                    Message = "Success.",
                });
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
