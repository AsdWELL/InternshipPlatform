using InternshipPlatform.Application.Dtos.StudentProfile;

namespace InternshipPlatform.Application.Dtos.StudentGroupApplication
{
    public class StudentGroupApplicationCuratorItem
    {
        public int Id { get; set; }

        public StudentProfileItem Student { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}
