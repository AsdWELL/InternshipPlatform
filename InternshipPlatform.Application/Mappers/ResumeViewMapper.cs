using InternshipPlatform.Application.Dtos.ResumeView;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class ResumeViewMapper
    {
        public static StudentResumeViewResponse ToStudentResponse(this ResumeView resumeView)
        {
            return new StudentResumeViewResponse
            {
                Id = resumeView.Id,
                ViewDate = resumeView.ViewDate,
                CompanyId = resumeView.CompanyId,
                CompanyName = resumeView.Company.Name,
                CompanyLogoPath = resumeView.Company.LogoPath
            };
        }

        public static EmployerResumeViewResponse ToEmployerResponse(this ResumeView resumeView)
        {
            return new EmployerResumeViewResponse
            {
                Id = resumeView.Id,
                ResumeId = resumeView.ResumeId,
                SpecializationName = resumeView.Resume.Specialization.Name,
                StudentId = resumeView.Resume.StudentId,
                StudentName = resumeView.Resume.StudentProfile.Name,
                StudentSurname = resumeView.Resume.StudentProfile.Surname,
                StudentPatronymic = resumeView.Resume.StudentProfile.Patronymic,
                StudentAvatarPath = resumeView.Resume.StudentProfile.AvatarPath,
                ViewDate = resumeView.ViewDate
            };
        }
    }
}
