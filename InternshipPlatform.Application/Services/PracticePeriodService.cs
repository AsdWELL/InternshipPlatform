using InternshipPlatform.Application.Dtos.PracticePeriod;
using InternshipPlatform.Application.Exceptions.PracticePeriod;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;

namespace InternshipPlatform.Application.Services
{
    public class PracticePeriodService(IPracticePeriodRepository practicePeriodRepository) : IPracticePeriodService
    {
        private static int GetCurrentAcademicYearStart()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            return today.Month >= 9
                ? today.Year
                : today.Year - 1;
        }

        public async Task<List<StudentPracticePeriodItem>> GetStudentPracticePeriods(int studentId)
        {
            var practicePeriods = await practicePeriodRepository.GetStudentPracticePeriods(studentId);

            return practicePeriods
                .Select(p => p.ToStudentItem())
                .OrderByDescending(p => p.IsActive)
                .ThenByDescending(p => p.AcademicYearStart)
                .ThenByDescending(p => p.StartDate)
                .ToList();
        }

        public async Task<StudentPracticePeriodDetails> GetStudentPracticePeriodDetails(int studentId, int practicePeriodId)
        {
            var practicePeriod = await practicePeriodRepository.GetStudentPracticePeriodDetails(studentId, practicePeriodId)
                ?? throw new PracticePeriodNotFoundException();

            return practicePeriod.ToStudentDetails();
        }

        public async Task<List<TeacherPracticeGroupItem>> GetTeacherCurrentYearPracticeGroups(int teacherId)
        {
            var currentAcademicYearStart = GetCurrentAcademicYearStart();

            var practicePeriods = await practicePeriodRepository.GetTeacherPracticePeriodsByAcademicYear(
                teacherId,
                currentAcademicYearStart);

            return practicePeriods
                .SelectMany(p => p.StudentGroups.Select(g => p.ToTeacherGroupItem(g)))
                .OrderByDescending(g => g.IsActive)
                .ThenBy(g => g.StartDate)
                .ThenBy(g => g.GroupName)
                .ToList();
        }

        public async Task<TeacherPracticeGroupDetails> GetTeacherPracticeGroupDetails(int teacherId, int practicePeriodId, int groupId)
        {
            var practicePeriod = await practicePeriodRepository.GetTeacherPracticePeriodWithGroup(
                teacherId,
                practicePeriodId,
                groupId) ?? throw new PracticePeriodNotFoundException();

            var group = practicePeriod.StudentGroups.First(g => g.Id == groupId);

            return practicePeriod.ToTeacherGroupDetails(group);
        }
    }
}
