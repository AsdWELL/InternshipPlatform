using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IUniversityRepository
    {
        Task<bool> IsUniversityExists(int universityId);

        Task<List<University>> GetAllUniversities();

        Task<List<University>> SearchUniversities(string universityName);
    }
}
