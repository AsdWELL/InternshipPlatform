using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface ISpecializationRepository
    {
        Task<bool> IsSpecializationExists(int id);

        Task<List<Specialization>> GetAllSpecializations();

        Task<List<Specialization>> SearchSpecialization(string specializationName);
    }
}
