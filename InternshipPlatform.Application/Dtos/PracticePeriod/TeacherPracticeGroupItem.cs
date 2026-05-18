namespace InternshipPlatform.Application.Dtos.PracticePeriod
{
    public class TeacherPracticeGroupItem
    {
        public int PracticePeriodId { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public int CourseNumber { get; set; }

        public int AcademicYearStart { get; set; }

        public int AcademicYearEnd => AcademicYearStart + 1;

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public int StudentsCount { get; set; }

        public bool IsActive { get; set; }
    }
}