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

        // GET: api/<PublishersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("publishers/{id}")]
        //[Authorize(Roles = "Publisher")]
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
        [HttpPost("publisher-profile/{id}")]
        [MapToApiVersion(1)]
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

        // DELETE api/<PublishersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
