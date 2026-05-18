namespace InternshipPlatform.Application.Dtos.PracticePeriod
{
    public class StudentPracticePeriodDetails
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

        public int SupervisorId { get; set; }

        public string SupervisorName { get; set; }

        public string SupervisorSurname { get; set; }

        public string? SupervisorPatronymic { get; set; }

        public string? SupervisorEmail { get; set; }
    }
}