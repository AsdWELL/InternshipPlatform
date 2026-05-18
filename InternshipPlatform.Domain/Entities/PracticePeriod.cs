namespace InternshipPlatform.Domain.Entities
{
    public class PracticePeriod
    {
        public int Id { get; set; }

        public int SupervisorId { get; set; }

        public Teacher Supervisor { get; set; }

        public int EducationalProgramId { get; set; }

        public EducationalProgram EducationalProgram { get; set; }

        public int CourseNumber { get; set; }

        public int AcademicYearStart { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public List<StudentGroup> StudentGroups { get; set; } = [];
    }
}
