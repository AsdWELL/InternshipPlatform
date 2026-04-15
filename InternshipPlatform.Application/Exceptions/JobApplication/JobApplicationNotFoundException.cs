using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.JobApplication
{
    public class JobApplicationNotFoundException() : NotFoundException("Отклик не найден");
}
