using InternshipPlatform.Application.Dtos.JobApplication;
using InternshipPlatform.Application.Values;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class JobApplicationMapper
    {
        public static JobApplication ToDomain(
            this CreateJobApplicaionRequest request,
            string role,
            int chatId)
        {
            return new JobApplication
            {
                ResumeId = request.ResumeId,
                VacancyId = request.VacancyId,
                LastStatusDate = DateOnly.FromDateTime(DateTime.UtcNow),
                ApplicationStatusId = role switch
                {
                    Roles.Student => (int)JobApplicationStatuses.Pending,
                    Roles.Employer => (int)JobApplicationStatuses.InterviewInvited,
                    _ => throw new InvalidOperationException("Указана неправильная роль для создания отклика")
                },
                ChatId = chatId
            };
        }

        public static StudentApplicationResponse ToStudentApplication(this JobApplication application)
        {
            return new StudentApplicationResponse
            {
                Id = application.Id,
                VacancyId = application.VacancyId,
                VacancyTitle = application.Vacancy.Title,
                LastStatusDate = application.LastStatusDate,
                ApplicationStatus = application.ApplicationStatus.Name,
                CompanyId = application.Vacancy.CompanyId,
                CompanyName = application.Vacancy.Company.Name,
                CompanyLogoPath = application.Vacancy.Company.LogoPath,
                ChatId = application.ChatId
            };
        }

        public static EmployerApplicationResponse ToEmployerApplication(this JobApplication application)
        {
            return new EmployerApplicationResponse
            {
                Id = application.Id,
                ResumeId = application.ResumeId,
                SpecializationName = application.Resume.Specialization.Name,
                ApplicationStatus = application.ApplicationStatus.Name,
                LastStatusDate = application.LastStatusDate,
                StudentId = application.Resume.StudentId,
                StudentName = application.Resume.StudentProfile.Name,
                StudentSurname = application.Resume.StudentProfile.Surname,
                StudentPatronymic = application.Resume.StudentProfile.Patronymic,
                StudentAvatarPath = application.Resume.StudentProfile.AvatarPath,
                ChatId = application.ChatId
            };
        }
    }
}
