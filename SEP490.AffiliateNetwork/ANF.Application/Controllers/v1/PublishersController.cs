using ANF.Core.Commons;
using ANF.Core.Models.Requests;
using ANF.Core.Services;
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

        // GET api/<PublishersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// Add profile to new publisher
        /// </summary>
        /// <param name="id">Publisher's id</param>
        /// <param name="value">Data</param>
        /// <returns></returns>
        [HttpPost("publisher-profile/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddProfile(long id, [FromBody] PublisherProfileRequest value)
        {
            if (id != value.PublisherId) return BadRequest();
            var result = await _publisherService.AddProfile(value);
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
