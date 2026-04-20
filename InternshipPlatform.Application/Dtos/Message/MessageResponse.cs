namespace InternshipPlatform.Application.Dtos.Message
{
    public class MessageResponse
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsRead { get; set; }

        public int SenderUserId { get; set; }
    }
}
