namespace InternshipPlatform.Application.Dtos.User
{
    public class RegisterCompanyRequest
    {
        public string Email { get; set; }

        public string CompanyName { get; set; }

        public string Inn { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }
    }
}
