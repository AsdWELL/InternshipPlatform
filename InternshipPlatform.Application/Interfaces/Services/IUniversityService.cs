using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IUniversityService
    {
        Task<List<University>> GetAllUniversities();

        Task<List<University>> SearchUniversities(string universityName);
    }
}
