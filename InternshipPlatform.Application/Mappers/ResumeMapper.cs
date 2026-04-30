using InternshipPlatform.Application.Dtos.Resume;
using InternshipPlatform.Application.Utils;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class ResumeMapper
    {
        public static Resume ToDomain(
            this CreateResumeRequest request,
            List<Skill> skills)
        {
            return new Resume
            {
                IsActive = true,
                DesiredSalary = request.DesiredSalary,
                LastUpdateDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Description = StringNormalizer.NormalizeOptional(request.Description),
                Region = StringNormalizer.NormalizeOptional(request.Region),
                SpecializationId = request.SpecializationId,
                StudentId = request.StudentId,
                Skills = skills
            };
        }

        public static ResumeItem ToItem(this Resume resume)
        {
            var totalWorkExperienceMonths = resume.WorkExperiences.Sum(x =>
                WorkExperienceUtils.CalculateExperienceMonths(x.StartDateWork, x.EndDateWork));

            return new ResumeItem
            {
                Id = resume.Id,
                StudentId = resume.StudentId,
                StudentName = resume.StudentProfile.Name,
                StudentSurname = resume.StudentProfile.Surname,
                StudentPatronymic = resume.StudentProfile.Patronymic,
                LastUpdateDate = resume.LastUpdateDate,
                DesiredSalary = resume.DesiredSalary,
                Region = resume.Region,
                SpecializationName = resume.Specialization.Name,
                TotalWorkExperienceMonths = totalWorkExperienceMonths,
                WorkExperiences = resume.WorkExperiences.Select(x => x.ToItem()).ToList(),
            };
        }

        public static ResumeOwnerItem ToOwnerItem(
            this Resume resume,
            int viewsCount)
        {
            var totalWorkExperienceMonths = resume.WorkExperiences.Sum(x =>
                WorkExperienceUtils.CalculateExperienceMonths(x.StartDateWork, x.EndDateWork));

            return new ResumeOwnerItem
            {
                Id = resume.Id,
                LastUpdateDate = resume.LastUpdateDate,
                IsActive = resume.IsActive,
                DesiredSalary = resume.DesiredSalary,
                Region = resume.Region,
                SpecializationName = resume.Specialization.Name,
                TotalWorkExperienceMonths = totalWorkExperienceMonths,
                WorkExperiences = resume.WorkExperiences.Select(x => x.ToItem()).ToList(),
                ApplicationsCount = resume.Applications.Count,
                ViewsCount = viewsCount
            };
        }

        public static ResumeDetails ToDetails(this Resume resume)
        {
            var totalWorkExperienceMonths = resume.WorkExperiences.Sum(x =>
                WorkExperienceUtils.CalculateExperienceMonths(x.StartDateWork, x.EndDateWork));

            return new ResumeDetails
            {
                Id = resume.Id,
                LastUpdateDate = resume.LastUpdateDate,
                Description = resume.Description,
                IsActive = resume.IsActive,
                DesiredSalary = resume.DesiredSalary,
                Region = resume.Region,
                Specialization = resume.Specialization,
                Skills = resume.Skills,
                TotalWorkExperienceMonths = totalWorkExperienceMonths,
                StudentProfile = resume.StudentProfile.ToResponse(),
                WorkExperiences = resume.WorkExperiences.Select(we => we.ToDetails()).ToList()
            };
        }
    }
}
