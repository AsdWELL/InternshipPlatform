using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Services
{
    public class UniversityService(IUniversityRepository universityRepository) : IUniversityService
    {
        public async Task<List<University>> GetAllUniversities()
        {
            return await universityRepository.GetAllUniversities();
        }

        public async Task<List<University>> SearchUniversities(string universityName)
        {
            return await universityRepository.SearchUniversities(universityName);
        }
    }
}
