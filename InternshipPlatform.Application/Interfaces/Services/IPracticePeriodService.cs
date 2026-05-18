using InternshipPlatform.Application.Dtos.PracticePeriod;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IPracticePeriodService
    {
        Task<List<StudentPracticePeriodItem>> GetStudentPracticePeriods(int studentId);

        Task<StudentPracticePeriodDetails> GetStudentPracticePeriodDetails(int studentId, int practicePeriodId);

        Task<List<TeacherPracticeGroupItem>> GetTeacherCurrentYearPracticeGroups(int teacherId);

        Task<TeacherPracticeGroupDetails> GetTeacherPracticeGroupDetails(int teacherId, int practicePeriodId, int groupId);
    }
}