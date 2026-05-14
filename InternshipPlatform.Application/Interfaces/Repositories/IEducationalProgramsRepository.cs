using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IEducationalProgramsRepository
    {
        Task<EducationalProgram?> GetEducationalProgramById(int id);

        Task<List<EducationalProgram>> GetAllUniversityEducationalPrograms(int teacherId);

        Task<List<EducationalProgram>> SearchEducationalPrograms(int teacherId, string educationalProgramName);
    }
}
