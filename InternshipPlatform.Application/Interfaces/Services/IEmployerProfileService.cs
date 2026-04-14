using InternshipPlatform.Application.Dtos.EmployerProflie;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IEmployerProfileService
    {
        Task<EmployerProflieResponse> GetEmployerProfileById(int employerId);

        Task UpdateEmployerProfile(UpdateEmployerProfileRequest request);

        Task Logout(int employerId);

        Task DeleteEmployerProfile(int employerId);
    }
}
