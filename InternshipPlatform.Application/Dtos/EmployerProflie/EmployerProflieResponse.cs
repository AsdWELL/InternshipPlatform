using InternshipPlatform.Application.Dtos.Company;

namespace InternshipPlatform.Application.Dtos.EmployerProflie
{
    public class EmployerProflieResponse
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public bool IsVerified { get; set; }

        public CompanyResponse Company { get; set; }
    }
}
