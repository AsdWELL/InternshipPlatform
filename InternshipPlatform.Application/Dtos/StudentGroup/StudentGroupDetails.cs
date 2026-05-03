using InternshipPlatform.Application.Dtos.StudentProfile;

namespace InternshipPlatform.Application.Dtos.StudentGroup
{
    public class StudentGroupDetails
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Specialization { get; set; }

        public int EnrollmentYear { get; set; }

        public int GraduationYear { get; set; }

        public string InviteCode { get; set; }

        public int StudentsCount { get; set; }

        public List<StudentProfileItem> Students { get; set; }
    }
}
