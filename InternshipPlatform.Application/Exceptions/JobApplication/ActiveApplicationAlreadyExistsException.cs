using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.JobApplication
{
    public class ActiveApplicationAlreadyExistsException() 
        : ConflictException("ResumeId", "Активный отклик на вакансию уже существует");
}
