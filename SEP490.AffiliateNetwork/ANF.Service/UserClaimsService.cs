using ANF.Core.Services;
using Microsoft.AspNetCore.Http;

namespace ANF.Service
{
    public class UserClaimsService(IHttpContextAccessor httpContextAccessor) : IUserClaimsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string GetClaim(string key)
        {
            var claims = GetClaims();
            return claims.TryGetValue(key, out var value) ? value : string.Empty;
        }

        public IDictionary<string, string> GetClaims()
            => _httpContextAccessor.HttpContext?.Items["UserClaims"] as IDictionary<string, string> ?? new Dictionary<string, string>();
    }
}
