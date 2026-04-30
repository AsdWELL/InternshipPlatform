using InternshipPlatform.Application.Dtos.VacancyView;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class VacancyViewMapper
    {
        public static EmployerVacancyViewResponse ToEmployerResponse(this VacancyView vacancyView)
        {
            return new EmployerVacancyViewResponse
            {
                Id = vacancyView.Id,
                StudentId = vacancyView.StudentId,
                StudentName = vacancyView.StudentProfile.Name,
                StudentSurname = vacancyView.StudentProfile.Surname,
                StudentPatronymic = vacancyView.StudentProfile.Patronymic,
                StudentAvatarPath = vacancyView.StudentProfile.AvatarPath,
                ViewDate = vacancyView.ViewDate
            };
        }

        public static StudentVacancyViewResponse ToStudentResponse(this VacancyView vacancyView)
        {
            return new StudentVacancyViewResponse
            {
                Id = vacancyView.Id,
                CompanyId = vacancyView.Vacancy.CompanyId,
                CompanyName = vacancyView.Vacancy.Company.Name,
                CompanyLogoPath = vacancyView.Vacancy.Company.LogoPath,
                VacancyId = vacancyView.VacancyId,
                VacancyTitle = vacancyView.Vacancy.Title,
                ViewDate = vacancyView.ViewDate
            };
        }
    }
}
