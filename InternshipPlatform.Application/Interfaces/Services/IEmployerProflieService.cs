using InternshipPlatform.Application.Dtos.EmployerProflie;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IEmployerProflieService
    {
        Task<EmployerProflieResponse> GetEmployerProfileById(int employerId);

        Task UpdateEmployerProfile(UpdateEmployerProflieRequest request);

        Task Logout(int employerId);

        Task DeleteEmployerProfile(int employerId);
    }
}
