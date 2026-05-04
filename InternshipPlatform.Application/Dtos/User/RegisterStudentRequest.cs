namespace InternshipPlatform.Application.Dtos.User
{
    public class RegisterStudentRequest
    {
        public string Name { get; set; }

        public string Surname { get; set; }
        
        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }

        public string? InviteCode { get; set; }
    }
}
