namespace InternshipPlatform.Application.Dtos.Company
{
    public class CompanyResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Inn { get; set; }

        public string? Link { get; set; }

        public string? Description { get; set; }

        public string? LogoPath { get; set; }
    }
}
