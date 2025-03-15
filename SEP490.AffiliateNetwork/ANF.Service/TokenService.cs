using ANF.Core.Commons;
using ANF.Core.Models.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ANF.Service
{
    public sealed class TokenService(IOptions<JwtOptions> jwtOptions)
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value ??
            throw new ArgumentNullException(nameof(jwtOptions));

        public string GenerateToken(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var key = Encoding.ASCII.GetBytes(_jwtOptions.Key);
            var securityKey = new SymmetricSecurityKey(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                ]),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
