namespace InternshipPlatform.Domain.Entities
{
    public class PracticeApplication
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public int StudentId { get; set; }

        public StudentProfile Student { get; set; }

        public int PracticePeriodId { get; set; }

        public PracticePeriod PracticePeriod { get; set; }

        public int PracticeOfferId { get; set; }

        public PracticeOffer PracticeOffer { get; set; }
    }
}
