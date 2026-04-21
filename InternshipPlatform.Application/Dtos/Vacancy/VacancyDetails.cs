using InternshipPlatform.Application.Dtos.Company;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Dtos.Vacancy
{
    public class VacancyDetails
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public int SalaryFrom { get; set; }

        public int SalaryTo { get; set; }

        public bool IsRemote { get; set; }

        public string? Region { get; set; }

        public bool IsActive { get; set; }

        public int MinWorkExperienceYears { get; set; }

        public Specialization Specialization { get; set; }

        public CompanyResponse Company { get; set; }

        public List<Skill> Skills { get; set; } 
    }
}
