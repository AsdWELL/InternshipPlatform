using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.Message
{
    public class SendMessageRequest
    {
        [JsonIgnore]
        public int SenderUserId { get; set; }

        [JsonIgnore]
        public string Role { get; set; }

        public int ChatId { get; set; }

        public string Content { get; set; }
    }
}
