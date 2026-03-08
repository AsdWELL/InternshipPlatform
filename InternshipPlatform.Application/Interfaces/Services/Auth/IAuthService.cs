using InternshipPlatform.Application.Dtos.Auth;
using InternshipPlatform.Application.Dtos.User;

namespace InternshipPlatform.Application.Interfaces.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResponse> Register(RegisterUserRequest request);

        Task<AuthResponse> Login(LoginUserRequest request);

        Task<AuthResponse> RefreshToken(string refreshToken);
    }
}
