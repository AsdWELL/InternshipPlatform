namespace InternshipPlatform.Domain.Entities
{
    public class StudentProfile
    {
        public int UserId { get; set; }
        
        public User User { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string? Patronymic { get; set; }

        public DateOnly? BirthdayDate { get; set; }

        public string? Phone { get; set; }

        public string? VkLink { get; set; }

        public string? TgLink { get; set; }

        public string? MaxLink { get; set; }

        public string? GithubLink { get; set; }

        public string? University { get; set; }

        public string? Specialization { get; set; }

        public int? GraduationYear { get; set; }

        public string? AvatarPath { get; set; }
    }
}
