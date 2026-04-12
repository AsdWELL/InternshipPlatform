using InternshipPlatform.Application.Dtos.Vacancy;
using InternshipPlatform.Application.Utils;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class VacancyMapper
    {
        public static Vacancy ToDomain(
            this CreateVacancyRequest request,
            int companyId,
            List<Skill> skills)
        {
            return new Vacancy
            {
                CompanyId = companyId,
                Title = StringNormalizer.NormalizeRequired(request.Title),
                Description = StringNormalizer.NormalizeOptional(request.Description),
                SalaryFrom = request.SalaryFrom,
                SalaryTo = request.SalaryTo,
                IsRemote = request.IsRemote,
                ViewsCount = 0,
                IsActive = true,
                Region = StringNormalizer.NormalizeOptional(request.Region),
                MinWorkExperienceYears = request.MinWorkExperienceYears,
                SpecializationId = request.SpecializationId,
                Skills = skills
            };
        }

        public static VacancyItem ToItem(this Vacancy vacancy)
        {
            return new VacancyItem
            {
                Id = vacancy.Id,
                Title = vacancy.Title,
                SalaryFrom = vacancy.SalaryFrom,
                SalaryTo = vacancy.SalaryTo,
                IsRemote = vacancy.IsRemote,
                MinWorkExperienceYears = vacancy.MinWorkExperienceYears,
                CompanyName = vacancy.Company.Name,
                CompanyLogoPath = vacancy.Company.LogoPath,
                Region = vacancy.Region
            };
        }

        public static VacancyDetails ToDetails(this Vacancy vacancy)
        {
            return new VacancyDetails
            {
                Id = vacancy.Id,
                Title = vacancy.Title,
                Description = vacancy.Description,
                SalaryFrom = vacancy.SalaryFrom,
                SalaryTo = vacancy.SalaryTo,
                IsRemote = vacancy.IsRemote,
                Region = vacancy.Region,
                MinWorkExperienceYears = vacancy.MinWorkExperienceYears,
                Specialization = vacancy.Specialization,
                Company = vacancy.Company.ToResponse(),
                Skills = vacancy.Skills
            };
        }
    }
}
