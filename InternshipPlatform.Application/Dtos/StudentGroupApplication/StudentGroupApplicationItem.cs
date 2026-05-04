namespace InternshipPlatform.Application.Dtos.StudentGroupApplication
{
    public class StudentGroupApplicationItem
    {
        public int Id { get; set; }

        public string University { get; set; }

        public string Specialization { get; set; }

        public string GroupName { get; set; }

        public string InviteCode { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
