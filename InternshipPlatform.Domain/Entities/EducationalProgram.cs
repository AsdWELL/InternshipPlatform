namespace InternshipPlatform.Domain.Entities
{
    public class EducationalProgram
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int UniversityId { get; set; }

        public string SpecializationCode { get; set; }

        public string GroupCode { get; set; }

        public int DurationYears { get; set; }
    }
}
