using InternshipPlatform.Application.Dtos.EmployerProflie;
using InternshipPlatform.Application.Exceptions.EmployerProfile;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Interfaces.Services.Auth;
using InternshipPlatform.Application.Mappers;

namespace InternshipPlatform.Application.Services
{
    public class EmployerProflieService(
        IEmployerProfileRepository employerProfileRepository,
        IPasswordHasher passwordHasher, 
        IUnitOfWork unitOfWork) : IEmployerProflieService
    {
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

            string? passwordHash = null;

            if (request.Password is not null)
                passwordHash = passwordHasher.Generate(request.Password);

            await employerProfileRepository.UpdateEmployerProfile(request.ToDomain(passwordHash));

            await unitOfWork.SaveChangesAsync();
        }
    }
}
