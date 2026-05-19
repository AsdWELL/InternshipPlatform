using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.PracticeApplication
{
    public class CurrentSemesterPracticePeriodNotFoundException() 
        : NotFoundException("В текущем семестре нет практики, на которую можно записаться");
}
