namespace InternshipPlatform.Domain.Entities
{
    public class StudentGroup
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int UniversityId { get; set; }

        public University University { get; set; }

        public int EducationalProgramId { get; set; }

        public EducationalProgram EducationalProgram { get; set; }

        public int EnrollmentYear { get; set; }

        public int GraduationYear { get; set; }

        public string InviteCode { get; set; }

        public int CuratorId { get; set; }

        public Teacher Curator { get; set; }

        public List<StudentProfile> StudentProfiles { get; set; } = [];

        public List<PracticePeriod> PracticePeriods { get; set; } = [];
    }
}
