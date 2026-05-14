using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.StudentGroup
{
    public class CreateStudentGroupRequest
    {
        [JsonIgnore]
        public int CuratorId { get; set; }

        public int EducationalProgramId { get; set; }

        public int EnrollmentYear { get; set; }
    }
}