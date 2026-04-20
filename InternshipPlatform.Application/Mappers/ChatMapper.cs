using InternshipPlatform.Application.Dtos.Chat;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class ChatMapper
    {
        public static Chat ToDomain(
            this StartStudentChatRequest request,
            int companyId)
        {
            return new Chat
            {
                StudentId = request.StudentId,
                CompanyId = companyId,
                VacancyId = request.VacancyId,
                IsClosed = false
            };
        }

        public static Chat ToDomain(
            this StartEmployerChatRequest request,
            int companyId)
        {
            return new Chat
            {
                StudentId = request.StudentId,
                CompanyId = companyId,
                VacancyId = request.VacancyId,
                IsClosed = false
            };
        }

        public static StudentChatItem ToItem(this StudentChatResult chat)
        {
            return new StudentChatItem
            {
                Id = chat.Id,
                CompanyId = chat.CompanyId,
                CompanyName = chat.CompanyName,
                CompanyLogoPath = chat.CompanyLogoPath,
                VacancyId = chat.VacancyId,
                VacancyTitle = chat.VacancyTitle,
                LastMessage = chat.LastMessage?.ToResponse(),
                UnreadMessagesCount = chat.UnreadMessagesCount,
                IsClosed = chat.IsClosed
            };
        }

        public static EmployerChatItem ToItem(this EmployerChatResult chat)
        {
            return new EmployerChatItem
            {
                Id = chat.Id,
                StudentId = chat.StudentId,
                StudentName = chat.StudentName,
                StudentSurname = chat.StudentSurname,
                StudentPatronymic = chat.StudentPatronymic,
                StudentAvatarPath = chat.StudentAvatarPath,
                SpecializationName = chat.SpecializationName,
                VacancyId = chat.VacancyId,
                VacancyTitle = chat.VacancyTitle,
                LastMessage = chat.LastMessage?.ToResponse(),
                UnreadMessagesCount = chat.UnreadMessagesCount,
                IsClosed = chat.IsClosed
            };
        }

        public static StudentChatMessagesResponse ToStudentResponse(this Chat chat)
        {
            return new StudentChatMessagesResponse
            {
                Id = chat.Id,
                CompanyId = chat.CompanyId,
                CompanyName = chat.Company.Name,
                CompanyLogoPath = chat.Company.LogoPath,
                VacancyId = chat.VacancyId,
                VacancyTitle = chat.Vacancy.Title,
                Messages = chat.Messages
                    .Select(m => m.ToResponse())
                    .OrderBy(m => m.CreatedAt)
                    .ToList(),
                IsClosed = chat.IsClosed
            };
        }

        public static EmployerChatMessagesResponse ToEmployerResponse(this Chat chat)
        {
            return new EmployerChatMessagesResponse
            {
                Id = chat.Id,
                StudentId = chat.StudentId,
                StudentName = chat.StudentProfile.Name,
                StudentSurname = chat.StudentProfile.Surname,
                StudentPatronymic = chat.StudentProfile.Patronymic,
                StudentAvatarPath = chat.StudentProfile.AvatarPath,
                SpecializationName = chat.Application?.Resume.Specialization.Name,
                VacancyId = chat.VacancyId,
                VacancyTitle = chat.Vacancy.Title,
                Messages = chat.Messages
                    .Select(m => m.ToResponse())
                    .OrderBy(m => m.CreatedAt)
                    .ToList(),
                IsClosed = chat.IsClosed
            };
        }
    }
}
