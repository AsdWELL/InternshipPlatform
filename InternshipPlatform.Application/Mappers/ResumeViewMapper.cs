using InternshipPlatform.Application.Dtos.ResumeView;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class ResumeViewMapper
    {
        public static ResumeViewResponse ToResponse(this ResumeView resumeView)
        {
            return new ResumeViewResponse
            {
                Id = resumeView.Id,
                ViewDate = resumeView.ViewDate,
                CompanyName = resumeView.Company.Name,
                CompanyLogoPath = resumeView.Company.LogoPath
            };
        }
    }
}
