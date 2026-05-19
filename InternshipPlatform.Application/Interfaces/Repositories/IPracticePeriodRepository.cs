using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IPracticePeriodRepository
    {
        Task<PracticePeriod?> GetCurrentStudentPracticePeriod(int studentId);

        Task<List<PracticePeriod>> GetStudentPracticePeriods(int studentId);

        Task<PracticePeriod?> GetStudentPracticePeriodDetails(int studentId, int practicePeriodId);

        Task<List<PracticePeriod>> GetTeacherPracticePeriodsByAcademicYear(int teacherId, int academicYearStart);

        Task<PracticePeriod?> GetTeacherPracticePeriodWithGroup(int teacherId, int practicePeriodId, int groupId);
    }
}