namespace InternshipPlatform.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsRead { get; set; }

        public int ChatId { get; set; }

        public int SenderUserId { get; set; }
    }
}
