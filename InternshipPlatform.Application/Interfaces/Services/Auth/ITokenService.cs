using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Services.Auth
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();
    }
}
