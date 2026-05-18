namespace InternshipPlatform.Application.Dtos.PracticePeriod
{
    public class StudentPracticePeriodItem
    {
        public int Id { get; set; }

        public int EducationalProgramId { get; set; }

        public string EducationalProgramName { get; set; }

        public int CourseNumber { get; set; }

        public int AcademicYearStart { get; set; }

        public int AcademicYearEnd => AcademicYearStart + 1;

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}