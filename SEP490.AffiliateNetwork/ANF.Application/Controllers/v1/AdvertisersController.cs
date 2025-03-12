using ANF.Core.Commons;
using ANF.Core.Models.Requests;
using ANF.Core.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    public class AdvertisersController(IAdvertiserService advertiserService) : BaseApiController
    {
        private readonly IAdvertiserService _advertiserService = advertiserService;

        // GET: api/<AdvertisersController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<AdvertisersController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
        
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

        // PUT api/<AdvertisersController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<AdvertisersController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
