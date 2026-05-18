using InternshipPlatform.Application.Dtos.PracticePeriod;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class PracticePeriodMapper
    {
        private static bool IsActive(this PracticePeriod practicePeriod)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            return practicePeriod.StartDate <= today &&
                   practicePeriod.EndDate >= today;
        }

        public static StudentPracticePeriodItem ToStudentItem(this PracticePeriod practicePeriod)
        {
            return new StudentPracticePeriodItem
            {
                Id = practicePeriod.Id,
                EducationalProgramId = practicePeriod.EducationalProgramId,
                EducationalProgramName = practicePeriod.EducationalProgram.Name,
                CourseNumber = practicePeriod.CourseNumber,
                AcademicYearStart = practicePeriod.AcademicYearStart,
                StartDate = practicePeriod.StartDate,
                EndDate = practicePeriod.EndDate,
                IsActive = practicePeriod.IsActive()
            };
        }

        public static StudentPracticePeriodDetails ToStudentDetails(this PracticePeriod practicePeriod)
        {
            return new StudentPracticePeriodDetails
            {
                Id = practicePeriod.Id,
                EducationalProgramId = practicePeriod.EducationalProgramId,
                EducationalProgramName = practicePeriod.EducationalProgram.Name,
                CourseNumber = practicePeriod.CourseNumber,
                AcademicYearStart = practicePeriod.AcademicYearStart,
                StartDate = practicePeriod.StartDate,
                EndDate = practicePeriod.EndDate,
                IsActive = practicePeriod.IsActive(),
                SupervisorId = practicePeriod.SupervisorId,
                SupervisorName = practicePeriod.Supervisor.Name,
                SupervisorSurname = practicePeriod.Supervisor.Surname,
                SupervisorPatronymic = practicePeriod.Supervisor.Patronymic,
                SupervisorEmail = practicePeriod.Supervisor.User.Email
            };
        }

        public static TeacherPracticeGroupItem ToTeacherGroupItem(
            this PracticePeriod practicePeriod,
            StudentGroup group)
        {
            return new TeacherPracticeGroupItem
            {
                PracticePeriodId = practicePeriod.Id,
                GroupId = group.Id,
                GroupName = group.Name,
                CourseNumber = practicePeriod.CourseNumber,
                AcademicYearStart = practicePeriod.AcademicYearStart,
                StartDate = practicePeriod.StartDate,
                EndDate = practicePeriod.EndDate,
                StudentsCount = group.StudentProfiles.Count,
                IsActive = practicePeriod.IsActive()
            };
        }

        public static TeacherPracticeGroupDetails ToTeacherGroupDetails(
            this PracticePeriod practicePeriod,
            StudentGroup group)
        {
            return new TeacherPracticeGroupDetails
            {
                PracticePeriodId = practicePeriod.Id,
                GroupId = group.Id,
                GroupName = group.Name,
                EducationalProgramName = practicePeriod.EducationalProgram.Name,
                CourseNumber = practicePeriod.CourseNumber,
                AcademicYearStart = practicePeriod.AcademicYearStart,
                StartDate = practicePeriod.StartDate,
                EndDate = practicePeriod.EndDate,
                Students = group.StudentProfiles
                    .OrderBy(sp => sp.Surname)
                    .ThenBy(sp => sp.Name)
                    .Select(sp => sp.ToTeacherPracticeGroupStudentItem())
                    .ToList()
            };
        }

        public static TeacherPracticeGroupStudentItem ToTeacherPracticeGroupStudentItem(
            this StudentProfile studentProfile)
        {
            return new TeacherPracticeGroupStudentItem
            {
                StudentId = studentProfile.UserId,
                Name = studentProfile.Name,
                Surname = studentProfile.Surname,
                Patronymic = studentProfile.Patronymic,
                AvatarPath = studentProfile.AvatarPath,
                Email = studentProfile.User.Email
            };
        }
    }
}
