using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.JobApplication
{
    public class InvalidJobApplicationStatusException() 
        : ConflictException("ApplicationStatus", "Указан некорректный статус для обновления отклика");
}
