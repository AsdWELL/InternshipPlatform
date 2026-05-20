using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.SubmissionComment
{
    public class SendSubmissionCommentByEmployerRequest
    {
        [JsonIgnore]
        public int EmployerId { get; set; }

        [JsonIgnore]
        public int StudentPracticeId { get; set; }

        public string Content { get; set; }
    }
}
