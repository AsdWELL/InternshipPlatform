using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IEducationalProgramsService
    {
        Task<List<EducationalProgram>> GetAllUniversityEducationalPrograms(int teacherId);

        Task<List<EducationalProgram>> SearchUniversityEducationalPrograms(int teacherId, string educationalProgramName);
    }
}
