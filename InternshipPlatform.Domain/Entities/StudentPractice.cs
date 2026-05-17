namespace InternshipPlatform.Domain.Entities
{
    public class StudentPractice
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public StudentProfile Student {  get; set; }

        public int PracticePeriodId { get; set; }

        public PracticePeriod PracticePeriod { get; set; }

        public int PracticeOfferId { get; set; }

        public PracticeOffer PracticeOffer { get; set; }

        public PracticeSubmission? PracticeSubmission { get; set; }
    }
}
