using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.PracticeApplication
{
    public class CreatePracticeApplicationRequest
    {
        [JsonIgnore]
        public int StudentId { get; set; }

        public int PracticeOfferId { get; set; }
    }
}