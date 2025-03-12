using ANF.Core.Commons;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    public class PublishersController(IPublisherService publisherService) : BaseApiController
    {
        private readonly IPublisherService _publisherService = publisherService;

        /// <summary>
        /// Get affiliate source of a publisher
        /// </summary>
        /// <param name="id">Publisher's id</param>
        /// <returns></returns>
        [HttpGet("publisher/{id}/affiliate-sources")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Publisher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAffiliateSourceOfPublisher(long id)
        {
            var response = await _publisherService.GetAffiliateSourceOfPublisher(id);
            return Ok(new ApiResponse<List<AffiliateSourceResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = response
            });
        }

        /// <summary>
        /// Get profile of a publisher
        /// </summary>
        /// <param name="id">Publisher's id</param>
        /// <returns></returns>
        [HttpGet("publishers/{id}/profile")]
        [Authorize(Roles = "Publisher")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPublisher(long id)
        {
            var publisher = await _publisherService.GetPublisherProfile(id);
            return Ok(new ApiResponse<PublisherResponse>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = publisher
            });
        }

        /// <summary>
        /// Add profile to new publisher
        /// </summary>
        /// <param name="id">Publisher's id</param>
        /// <param name="value">Data</param>
        /// <returns></returns>
        [HttpPost("publisher/{id}/profile")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Publisher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddProfile(long id, [FromBody] PublisherProfileRequest value)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }
            var result = await _publisherService.AddProfile(id, value);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Success."
            });
        }

        /// <summary>
        /// Add affiliate sources of a publisher
        /// </summary>
        /// <param name="id">Publisher's id</param>
        /// <param name="requests">The list of affiliate sources</param>
        /// <returns></returns>
        [HttpPost("publisher/{id}/affiliate-sources")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Publisher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddSources(long id, [FromBody] List<AffiliateSourceCreateRequest> requests)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }
            var result = await _publisherService.AddAffiliateSources(id, requests);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Success."
            });
        }

        /// <summary>
        /// Update affiliate source
        /// </summary>
        /// <param name="id">Affiliate source's id</param>
        /// <param name="request">Data</param>
        /// <returns></returns>
        [HttpPut("affiliate-source/{id}")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Publisher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAffiliateSource(long id, AffiliateSourceUpdateRequest request)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }
            var result = await _publisherService.UpdateAffiliateSource(id, request);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Success."
            });
        }

        /// <summary>
        /// Delete affiliate source
        /// </summary>
        /// <param name="id">Source's id</param>
        /// <returns></returns>
        [HttpDelete("affiliate-source/{id}")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Publisher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAffiliateSource(long id)
        {
            var result = await _publisherService.DeleteAffiliateSource(id);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Success."
            });
        }

        /// <summary>
        /// Delete affiliate sources
        /// </summary>
        /// <param name="sourceIds">The list of source's id</param>
        /// <returns></returns>
        [HttpDelete("affiliate-sources")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Publisher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAffiliateSources(List<long> sourceIds)
        {
            var result = await _publisherService.DeleteAffiliateSources(sourceIds);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Success."
            });
        }
    }
}
