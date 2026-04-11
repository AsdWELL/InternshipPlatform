using InternshipPlatform.Application.Dtos.EmployerProflie;
using InternshipPlatform.Application.Exceptions.Company;
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
        ICompanyRepository companyRepository,
        IPasswordHasher passwordHasher, 
        IUnitOfWork unitOfWork) : IEmployerProflieService
    {
        private async Task ThrowIfEmployerProfileNotExists(int employerId)
        {
            if (!await employerProfileRepository.IsEmployerProfileExists(employerId))
                throw new EmployerProflieNotFoundException();
        }
        
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
            var employer = await employerProfileRepository.GetEmployerProfileForUpdate(request.UserId)
                ?? throw new EmployerProflieNotFoundException();

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                await ThrowIfEmailAlreadyTaken(request);

                employer.User.Email = StringNormalizer.NormalizeToLower(request.Email);
            }

            if (!string.IsNullOrWhiteSpace(request.Password))
                employer.User.PasswordHash = passwordHasher.Generate(request.Password);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task Logout(int employerId)
        {
            await userRepository.UpdateRefreshToken(employerId, null, DateTime.UtcNow);
        }

        public async Task DeleteEmployerProfile(int employerId)
        {
            await ThrowIfEmployerProfileNotExists(employerId);
            
            var company = await companyRepository.GetCompanyByEmployerId(employerId)
                ?? throw new CompanyNotFoundException();
            
            await userRepository.DeleteUserById(employerId);

            await companyRepository.DeleteCompany(company.Id);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
