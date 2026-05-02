namespace InternshipPlatform.Domain.Entities
{
    public class StudentGroupRequest
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public StudentGroup Group { get; set; }

        public int StudentId { get; set; }

        public StudentProfile StudentProfile { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
