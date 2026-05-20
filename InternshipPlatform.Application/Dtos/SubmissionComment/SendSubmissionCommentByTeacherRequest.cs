using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.SubmissionComment
{
    public class SendSubmissionCommentByTeacherRequest
    {
        [JsonIgnore]
        public int TeacherId { get; set; }

        [JsonIgnore]
        public int StudentPracticeId { get; set; }

        public string Content { get; set; }
    }
}
