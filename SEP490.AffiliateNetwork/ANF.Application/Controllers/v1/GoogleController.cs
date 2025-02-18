using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace ANF.Application.Controllers.v1
{
    [ApiController]
    public class GoogleController : ControllerBase
    {
        [HttpGet("google-login")]
        [MapToApiVersion(1)]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleCallback", "Google");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // Callback endpoint for Google authentication
        [HttpGet("signin-google")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            var claims = result.Principal?.Identities
                .FirstOrDefault()?.Claims.Select(c => new { c.Type, c.Value });

            return Ok(new
            {
                Message = "Google authentication successful",
                UserClaims = claims
            });
        }
    }
}
