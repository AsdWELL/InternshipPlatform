namespace InternshipPlatform.Application.Dtos.User
{
    public class RegisterEmployerRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }
    }
}
