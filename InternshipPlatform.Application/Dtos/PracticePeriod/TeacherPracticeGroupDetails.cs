namespace InternshipPlatform.Application.Dtos.PracticePeriod
{
    public class TeacherPracticeGroupDetails
    {
        public int PracticePeriodId { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public string EducationalProgramName { get; set; }

        public int CourseNumber { get; set; }

        public int AcademicYearStart { get; set; }

        public int AcademicYearEnd => AcademicYearStart + 1;

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public List<TeacherPracticeGroupStudentItem> Students { get; set; }
    }
}