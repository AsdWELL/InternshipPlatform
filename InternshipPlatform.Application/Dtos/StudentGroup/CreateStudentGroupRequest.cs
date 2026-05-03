using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.StudentGroup
{
    public class CreateStudentGroupRequest
    {
        [JsonIgnore]
        public int CuratorId { get; set; }
        
        public string Name { get; set; }

        public string Specialization { get; set; }

        public int EnrollmentYear { get; set; }

        public int GraduationYear { get; set; }
    }
}