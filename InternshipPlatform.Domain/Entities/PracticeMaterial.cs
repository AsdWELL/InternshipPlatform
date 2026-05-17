namespace InternshipPlatform.Domain.Entities
{
    public class PracticeMaterial
    {
        public int Id { get; set; }

        public int PracticeOfferId { get; set; }

        public string Title { get; set; }

        public string FilePath { get; set; }
    }
}
