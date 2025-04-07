using ANF.Core.Commons;
using ANF.Core.Models.Requests;
using ANF.Core.Services;
using ANF.Service;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    public class PostbackController(IPostbackService postbackService) : BaseApiController
    {
        private readonly IPostbackService _postbackService = postbackService;

        [HttpPost("postbacks")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
                Message = "Create postback successfully"
            });
        }

    }
}
