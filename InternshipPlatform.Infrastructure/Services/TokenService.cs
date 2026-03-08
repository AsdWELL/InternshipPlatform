using InternshipPlatform.Application.Dtos.Auth;
using InternshipPlatform.Application.Interfaces.Services.Auth;
using InternshipPlatform.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InternshipPlatform.Infrastructure.Services
{
    public class TokenService(IOptions<TokenOptions> options) : ITokenService
    {
        private TokenOptions TokenOptions => options.Value;

        private List<Claim> CreateClaims(User user)
        {            
            var claims = new List<Claim> 
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            return claims;
        }

        public string GenerateAccessToken(User user)
        {
            var claims = CreateClaims(user);

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenOptions.Key)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                issuer: TokenOptions.Issuer,
                audience: TokenOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(TokenOptions.ExpiresAfterHours));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}
