using InternshipPlatform.Application.Dtos.Auth;
using InternshipPlatform.Application.Dtos.User;
using InternshipPlatform.Application.Exceptions.User;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services.Auth;
using InternshipPlatform.Application.Mappers;
using InternshipPlatform.Application.Values;
using InternshipPlatform.Domain.Entities;
using Microsoft.Extensions.Options;

namespace InternshipPlatform.Application.Services
{
    public class AuthService(
        IOptions<TokenOptions> tokenOptions,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IUserRepository userRepository,
        IStudentProfileRepository studentProfileRepository,
        ICompanyRepository companyRepository,
        IEmployerProfileRepository employerRepository,
        IUnitOfWork unitOfWork) : IAuthService
    {
        private TokenOptions TokenOptionsValue => tokenOptions.Value;
        
        private async Task ThrowIfEmailAlreadyTaken(string email)
        {
            if (await userRepository.GetUserByEmail(email) != null)
                throw new EmailAlreadyTakenException(email);
        }
        
        public async Task<AuthResponse> RegisterStudent(RegisterStudentRequest request)
        {
            var roleName = Roles.Student;

            await ThrowIfEmailAlreadyTaken(request.Email);

            var hashPassword = passwordHasher.Generate(request.Password);

            var refreshToken = tokenService.GenerateRefreshToken();

            var role = await userRepository.GetRoleByName(roleName);

            var user = request.ToUser(hashPassword, role,
                refreshToken, DateTime.UtcNow.AddHours(TokenOptionsValue.ExpiresAfterHours));

            await userRepository.AddUser(user);

            await studentProfileRepository.AddStudentProfile(
                request.ToStudentProfile(user));

            await unitOfWork.SaveChangesAsync();

            string accessToken = tokenService.GenerateAccessToken(user);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponse> RegisterEmployer(RegisterCompanyRequest request)
        {
            var roleName = Roles.Employer;

            await ThrowIfEmailAlreadyTaken(request.Email);

            var hashPassword = passwordHasher.Generate(request.Password);

            var refreshToken = tokenService.GenerateRefreshToken();

            var role = await userRepository.GetRoleByName(roleName);

            var user = request.ToUser(hashPassword, role,
                refreshToken, DateTime.UtcNow.AddHours(TokenOptionsValue.ExpiresAfterHours));

            await userRepository.AddUser(user);

            var company = request.ToCompany();

            await companyRepository.AddCompany(company);

            await employerRepository.AddEmployer(
                new EmployerProfile
                {
                    User = user,
                    Company = company
                });

            await unitOfWork.SaveChangesAsync();

            string accessToken = tokenService.GenerateAccessToken(user);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponse> Login(LoginUserRequest request)
        {
            var user = await userRepository.GetUserByEmail(request.Email)
                ?? throw new EmailNotFoundException(request.Email);

            if (!passwordHasher.Verify(request.Password, user.PasswordHash))
                throw new InvalidPasswordException();

            var accessToken = tokenService.GenerateAccessToken(user);

            var refreshToken = user.RefreshToken;

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                refreshToken = tokenService.GenerateRefreshToken();

                await userRepository.UpdateRefreshToken(user.Id,
                    refreshToken, DateTime.UtcNow.AddHours(TokenOptionsValue.ExpiresAfterHours));

                await unitOfWork.SaveChangesAsync();
            }

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponse> RefreshToken(string refreshToken)
        {
            var user = await userRepository.GetUserByRefreshToken(refreshToken)
                ?? throw new RefreshTokenNotFoundException();

            var newToken = tokenService.GenerateAccessToken(user);

            var newRefreshToken = tokenService.GenerateRefreshToken();

            await userRepository.UpdateRefreshToken(user.Id,
                newRefreshToken,
                DateTime.UtcNow.AddHours(TokenOptionsValue.ExpiresAfterHours));

            await unitOfWork.SaveChangesAsync();

            return new AuthResponse
            {
                AccessToken = newToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
