namespace InternshipPlatform.Domain.Entities
{
    public class SubmissionComment
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public int PracticeSubmissionId { get; set; }

        public int SenderId { get; set; }

        public User User { get; set; }

        public Company? Company { get; set; }

        public Teacher? Teacher { get; set; }
    }
}
