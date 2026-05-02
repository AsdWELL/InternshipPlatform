namespace InternshipPlatform.Application.Dtos.User
{
    public class RegisterCuratorRequest
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }

        public int UniversityId { get; set; }
    }
}
