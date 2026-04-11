using InternshipPlatform.Application.Dtos.Resume;
using InternshipPlatform.Application.Utils;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class WorkExperienceMapper
    {
        public static WorkExperienceItem ToItem(this WorkExperience workExperience)
        {
            var workDurationMonths = WorkExperienceUtils.CalculateExperienceMonths(
                workExperience.StartDateWork,
                workExperience.EndDateWork);

            return new WorkExperienceItem
            {
                Id = workExperience.Id,
                CompanyName = workExperience.CompanyName,
                StartDateWork = workExperience.StartDateWork,
                EndDateWork = workExperience.EndDateWork,
                Profession = workExperience.Profession,
                WorkDurationMonths = workDurationMonths
            };
        }

        public static WorkExperience ToDomain(this AddWorkExperienceRequest request)
        {
            return new WorkExperience
            {
                ResumeId = request.ResumeId,
                CompanyName = StringNormalizer.NormalizeRequired(request.CompanyName),
                Profession = StringNormalizer.NormalizeRequired(request.Profession),
                StartDateWork = request.StartDateWork,
                EndDateWork = request.EndDateWork,
                WorkDescription = StringNormalizer.NormalizeOptional(request.WorkDescription)
            };
        }
    }
}
