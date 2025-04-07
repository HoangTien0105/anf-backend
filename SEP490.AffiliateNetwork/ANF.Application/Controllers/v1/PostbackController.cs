using ANF.Core.Commons;
using ANF.Core.Models.Requests;
using ANF.Core.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    public class PostbackController(IPostbackService postbackService) : BaseApiController
    {
        private readonly IPostbackService _postbackService = postbackService;

        /// <summary>
        /// Create postback
        /// </summary>
        /// <param name="request">Postback Create Request</param>
        /// <returns></returns>
        [HttpPost("postbacks")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreatePostback([FromBody] PostbackRequest request)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }
            var result = await _postbackService.CreatePostBack(request);
            if (!result) return BadRequest();

            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Create postback successfully."
            });
        }

        /// <summary>
        /// Update postback's status
        /// </summary>
        /// <param name="id">Postback's id</param>
        /// <param name="status">Postback's status</param>
        /// <returns></returns>
        [HttpPatch("postbacks")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePostbackStatus(long id, string status)
        {
            var result = await _postbackService.UpdatePostBackStatus(id, status);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Update success."
            });
        }
    }
}
