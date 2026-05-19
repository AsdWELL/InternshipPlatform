using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.PracticeApplication
{
    public class StudentAlreadyEnrolledInPracticeException() 
        : ConflictException("PracticePeriodId", "Студент уже зачислен на практику в этом периоде");
}
