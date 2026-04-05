using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Services
{
    public class SpecializationService(ISpecializationRepository specializationRepository) : ISpecializationService
    {
        public Task<List<Specialization>> GetAllSpecializations()
        {
            return specializationRepository.GetAllSpecializations();
        }

        public Task<List<Specialization>> SearchSpecialization(string specializationName)
        {
            return specializationRepository.SearchSpecialization(specializationName);
        }
    }
}
