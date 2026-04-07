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

        public static Resume ToDomain(
            this UpdateResumeRequest request,
            List<Skill>? skills)
        {
            return new Resume
            {
                Id = request.Id,
                IsActive = request.IsActive ?? true,
                DesiredSalary = request.DesiredSalary,
                Region = request.Region,
                LastUpdateDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Description = StringNormalizer.NormalizeOptional(request.Description),
                SpecializationId = request.SpecializationId ?? int.MinValue,
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
                LastUpdateDate = resume.LastUpdateDate,
                DesiredSalary = resume.DesiredSalary,
                Region = resume.Region,
                SpecializationName = resume.Specialization.Name,
                TotalWorkExperienceMonths = totalWorkExperienceMonths,
                WorkExperiences = resume.WorkExperiences.Select(x => x.ToItem()).ToList(),
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
                DesiredSalary = resume.DesiredSalary,
                Region = resume.Region,
                Specialization = resume.Specialization,
                Skills = resume.Skills,
                TotalWorkExperienceMonths = totalWorkExperienceMonths,
                StudentProfile = resume.StudentProfile.ToResponse(),
                WorkExperiences = resume.WorkExperiences.Select(we => we.ToItem()).ToList()
            };
        }
    }
}
