namespace InternshipPlatform.Application.Dtos.StudentProfile
{
    public class StudentStatisticsResult
    {
        public Domain.Entities.StudentProfile StudentProfile { get; set; }

        public int ResumesCount { get; set; }

        public int ApplicationsCount { get; set; }
    }
}
