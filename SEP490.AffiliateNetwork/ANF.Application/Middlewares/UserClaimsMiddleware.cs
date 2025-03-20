
using System.IdentityModel.Tokens.Jwt;

namespace ANF.Application.Middlewares
{
    public class UserClaimsMiddleware(ILogger<UserClaimsMiddleware> logger) : IMiddleware
    {
        private readonly ILogger<UserClaimsMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                var claims = ExtractTokenClaims(context);
                if (claims.Any())
                {
                    context.Items["UserClaims"] = claims;
                }
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}");
                throw;
            }
        }

        private static IDictionary<string, string> ExtractTokenClaims(HttpContext context)
        {
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return new Dictionary<string, string>(); // Trả về danh sách rỗng nếu không có token
            }

            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
            {
                return new Dictionary<string, string>(); // Trả về danh sách rỗng nếu token không hợp lệ
            }

            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
        }
    }
}
