using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.StudentGroupApplication
{
    public class CreateStudentGroupApplicationRequest
    {
        [JsonIgnore]
        public int StudentId { get; set; }

        public string InviteCode { get; set; }
    }
}
