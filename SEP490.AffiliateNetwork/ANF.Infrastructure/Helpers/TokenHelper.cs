using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace ANF.Infrastructure.Helpers
{
    /// <summary>
    /// Retrieve claims from user's token
    /// </summary>
    public static class TokenHelper
    {
        public static IDictionary<string, string> GetTokenClaims(HttpContext httpContext)
        {
            var authorizationHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                throw new UnauthorizedAccessException("Bearer token is missing or invalid.");
            }

            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
            {
                throw new UnauthorizedAccessException("Invalid JWT token format.");
            }

            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);

            return claims;
        }
    }
}
