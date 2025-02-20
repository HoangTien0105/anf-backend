using ANF.Core.Commons;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    /// <summary>
    /// Controller for handling authentication-related actions.
    /// </summary>
    public class AuthController(IUserService userService) : BaseApiController
    {
        private readonly IUserService _userService = userService;

        /// <summary>
        /// Authenticates a user with the provided email and password.
        /// </summary>
        /// <param name="value">The login request containing email and password.</param>
        /// <returns>An ApiResponse containing the login response with user details and access token.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/v1/users/login
        ///     {
        ///         "email": "user@example.com",
        ///         "password": "password123"
        ///     }
        /// 
        /// Sample response:
        /// 
        ///     {
        ///         "isSuccess": true,
        ///         "message": "Login successfully.",
        ///         "value": {
        ///             "id": "d290f1ee-6c54-4b01-90e6-d701748f0851",
        ///             "firstName": "John",
        ///             "lastName": "Doe",
        ///             "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
        ///         }
        ///     }
        /// </remarks>
        [HttpPost("users/login")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest value)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = await _userService.Login(value.Email, value.Password);
            return Ok(new ApiResponse<LoginResponse>
            {
                IsSuccess = true,
                Message = "Login successfully.",
                Value = user
            });
        }
    }
}
