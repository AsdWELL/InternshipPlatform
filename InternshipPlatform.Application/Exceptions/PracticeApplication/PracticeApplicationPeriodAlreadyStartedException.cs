using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.PracticeApplication
{
    public class PracticeApplicationPeriodAlreadyStartedException()
        : ConflictException("PracticePeriodId", "Запись на практику закрыта, так как период практики уже начался");
}
