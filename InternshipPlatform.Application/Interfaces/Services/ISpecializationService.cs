using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface ISpecializationService
    {
        Task<List<Specialization>> GetAllSpecializations();

        Task<List<Specialization>> SearchSpecialization(string specializationName);
    }
}
