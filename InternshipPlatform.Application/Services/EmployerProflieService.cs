using InternshipPlatform.Application.Dtos.EmployerProflie;
using InternshipPlatform.Application.Exceptions.EmployerProfile;
using InternshipPlatform.Application.Exceptions.User;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Interfaces.Services.Auth;
using InternshipPlatform.Application.Mappers;
using InternshipPlatform.Application.Utils;

namespace InternshipPlatform.Application.Services
{
    public class EmployerProflieService(
        IUserRepository userRepository,
        IEmployerProfileRepository employerProfileRepository,
        IPasswordHasher passwordHasher, 
        IUnitOfWork unitOfWork) : IEmployerProflieService
    {
        private async Task ThrowIfEmailAlreadyTaken(UpdateEmployerProflieRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
                return;

            var normalizedEmail = StringNormalizer.NormalizeToLower(request.Email)!;

            var user = await userRepository.GetUserByEmail(normalizedEmail);

            if (user is not null && user.Id != request.UserId)
                throw new EmailAlreadyTakenException(nameof(request.Email), request.Email);
        }
        
        public async Task<EmployerProflieResponse> GetEmployerProfileById(int employerId)
        {
            var employerProfile = await employerProfileRepository.GetEmployerProfileById(employerId)
                ?? throw new EmployerProflieNotFoundException();

            return employerProfile.ToResponse();
        }

        public async Task UpdateEmployerProfile(UpdateEmployerProflieRequest request)
        {
            if (!await employerProfileRepository.IsEmployerProfileExists(request.UserId))
                throw new EmployerProflieNotFoundException();

            await ThrowIfEmailAlreadyTaken(request);

            string? passwordHash = null;

            if (request.Password is not null)
                passwordHash = passwordHasher.Generate(request.Password);

            await employerProfileRepository.UpdateEmployerProfile(request.ToDomain(passwordHash));

            await unitOfWork.SaveChangesAsync();
        }
    }
}
