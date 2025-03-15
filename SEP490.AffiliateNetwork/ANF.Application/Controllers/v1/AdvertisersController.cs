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
        [Authorize(Roles = "Advertiser")]
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
        public async Task<IActionResult> AddProfile(long id, [FromBody] AdvertiserProfileRequest value)
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
        /// Add bank accounts of an advertiser
        /// </summary>
        /// <param name="code"></param>
        /// <param name="value">Bank accounts' data</param>
        /// <returns></returns>
        [HttpPost("advertiser/{code}/bank-accounts")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Advertiser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddBankingInformation(Guid code, [FromBody] List<UserBankCreateRequest> value)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }
            var result = await _advertiserService.AddBankingInformation(code, value);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Success"
            });
        }
        
        /// <summary>
        /// Update advertiser's bank account information
        /// </summary>
        /// <param name="id">Bank account's id</param>
        /// <param name="request">Requested data to update</param>
        /// <returns></returns>
        [HttpPut("advertiser/bank-account/{id}")]
        [Authorize(Roles = "Advertiser")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBankingInformation(long id, [FromBody] UserBankUpdateRequest request)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
                return validationResult;
            var result = await _advertiserService.UpdateBankingInformation(id, request);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Update bank account successfully!"
            });
        }

        /// <summary>
        /// Delete bank account(s)
        /// </summary>
        /// <param name="ubIds">List user's banking id (advertiser)</param>
        /// <returns></returns>
        [HttpDelete("advertiser/bank-accounts")]
        [Authorize(Roles = "Advertiser")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBankingAccounts(List<long> ubIds)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
                return validationResult;
            var result = await _advertiserService.DeleteBankingInformation(ubIds);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Delete account successfully!"
            });
        }
    }
}
