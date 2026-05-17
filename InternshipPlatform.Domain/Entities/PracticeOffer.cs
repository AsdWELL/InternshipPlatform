namespace InternshipPlatform.Domain.Entities
{
    public class PracticeOffer
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public bool IsRemote { get; set; }

        public string? Region { get; set; }

        public bool IsActive { get; set; }

        public int SpecializationId { get; set; }

        public Specialization Specialization { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public int MaxStudents { get; set; }

        public List<PracticeMaterial> Materials { get; set; }
    }
}
