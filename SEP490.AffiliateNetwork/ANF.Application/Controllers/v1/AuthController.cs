using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using ANF.Core.Services;
using ANF.Core.Models.Requests;
using ANF.Core.Commons;
using ANF.Core.Models.Responses;

namespace ANF.Application.Controllers.v1
{
    public class AuthController(IUserService authService) : BaseApiController
    {
        private readonly IUserService _authService = authService;

        /// <summary>
        /// Authenticates a user with the provided email and password.
        /// </summary>
        /// <param name="request">The login request containing email and password.</param>
        /// <returns>An ApiResponse containing the login response with user details and access token.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/v1/user/login
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
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.Login(request.Email, request.Password);
            return Ok(new ApiResponse<LoginResponse>
            {
                IsSuccess = true,
                Message = "Login successfully.",
                Value = response
            });
        }

        // Endpoint to initiate Google login
        [HttpGet("google/init")]
        [MapToApiVersion(1)]
        public IActionResult Login()
        {
            var redirectUrl = Url.Action(nameof(HandleExternalLogin));
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // Endpoint to handle the callback after Google login
        [HttpGet("signin-google")]
        public async Task<IActionResult> HandleExternalLogin()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return Unauthorized();

            // Extract user information from claims
            var claims = authenticateResult.Principal?.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == "email")?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == "name")?.Value;

            return Ok(new { Email = email, Name = name });
        }

        // Endpoint to logout
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { Message = "Logged out successfully." });
        }
    }
}
