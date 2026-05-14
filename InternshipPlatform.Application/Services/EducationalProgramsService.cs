using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Services
{
    public class EducationalProgramsService(
        IEducationalProgramsRepository educationalProgramsRepository) : IEducationalProgramsService
    {
        public Task<List<EducationalProgram>> GetAllUniversityEducationalPrograms(int teacherId)
        {
            return educationalProgramsRepository.GetAllUniversityEducationalPrograms(teacherId);
        }

        public Task<List<EducationalProgram>> SearchUniversityEducationalPrograms(int teacherId, string educationalProgramName)
        {
            return educationalProgramsRepository.SearchEducationalPrograms(teacherId, educationalProgramName);
        }
    }
}
