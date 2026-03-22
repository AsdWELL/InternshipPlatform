namespace InternshipPlatform.Application.Dtos.StudentProfile
{
    public class StudentProfileResponse
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public bool IsVerified { get; set; }

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
