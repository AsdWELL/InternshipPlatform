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
                IsActive = true,
                Region = StringNormalizer.NormalizeOptional(request.Region),
                MinWorkExperienceYears = request.MinWorkExperienceYears,
                SpecializationId = request.SpecializationId,
                Skills = skills
            };
        }

        public static VacancyItem ToItem(
            this Vacancy vacancy,
            bool markAsFavorite = false)
        {
            return new VacancyItem
            {
                Id = vacancy.Id,
                Title = vacancy.Title,
                SalaryFrom = vacancy.SalaryFrom,
                SalaryTo = vacancy.SalaryTo,
                IsRemote = vacancy.IsRemote,
                Specialization = vacancy.Specialization.Name,
                MinWorkExperienceYears = vacancy.MinWorkExperienceYears,
                CompanyId = vacancy.CompanyId,
                CompanyName = vacancy.Company.Name,
                CompanyLogoPath = vacancy.Company.LogoPath,
                Region = vacancy.Region,
                IsFavorite = markAsFavorite
            };
        }

        public static VacancyOwnerItem ToOwnerItem(
            this Vacancy vacancy,
            int viewsCount)
        {
            return new VacancyOwnerItem
            {
                Id = vacancy.Id,
                Title = vacancy.Title,
                SalaryFrom = vacancy.SalaryFrom,
                SalaryTo = vacancy.SalaryTo,
                IsRemote = vacancy.IsRemote,
                Specialization = vacancy.Specialization.Name,
                MinWorkExperienceYears = vacancy.MinWorkExperienceYears,
                CompanyName = vacancy.Company.Name,
                CompanyLogoPath = vacancy.Company.LogoPath,
                Region = vacancy.Region,
                IsActive = vacancy.IsActive,
                ApplicationsCount = vacancy.Applications.Count,
                ViewsCount = viewsCount
            };
        }

        public static VacancyDetails ToDetails(
            this Vacancy vacancy,
            bool isFavorite)
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
                Skills = vacancy.Skills,
                IsFavorite = isFavorite
            };
        }

        public static VacancyOwnerDetails ToOwnerDetails(this Vacancy vacancy)
        {
            return new VacancyOwnerDetails
            {
                Id = vacancy.Id,
                Title = vacancy.Title,
                Description = vacancy.Description,
                SalaryFrom = vacancy.SalaryFrom,
                SalaryTo = vacancy.SalaryTo,
                IsRemote = vacancy.IsRemote,
                Region = vacancy.Region,
                IsActive = vacancy.IsActive,
                MinWorkExperienceYears = vacancy.MinWorkExperienceYears,
                Specialization = vacancy.Specialization,
                Company = vacancy.Company.ToResponse(),
                Skills = vacancy.Skills
            };
        }
    }
}
