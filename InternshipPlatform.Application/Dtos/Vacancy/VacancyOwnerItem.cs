namespace InternshipPlatform.Application.Dtos.Vacancy
{
    public class VacancyOwnerItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int? SalaryFrom { get; set; }

        public int? SalaryTo { get; set; }

        public bool IsRemote { get; set; }

        public int MinWorkExperienceYears { get; set; }

        public string CompanyName { get; set; }

        public string? CompanyLogoPath { get; set; }

        public string? Region { get; set; }

        public bool IsActive { get; set; }
    }
}
