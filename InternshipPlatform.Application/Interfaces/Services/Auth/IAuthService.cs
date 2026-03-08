using InternshipPlatform.Application.Dtos.Auth;
using InternshipPlatform.Application.Dtos.User;

namespace InternshipPlatform.Application.Interfaces.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterStudent(RegisterStudentRequest request);

        Task<AuthResponse> RegisterEmployer(RegisterEmployerRequest request);

        Task<AuthResponse> Login(LoginUserRequest request);

        Task<AuthResponse> RefreshToken(string refreshToken);
    }
}
