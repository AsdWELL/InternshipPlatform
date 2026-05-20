using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.StudentPractice
{
    public class PracticeHasNotStartedException() : BadRequestException("Практика ещё не началась");
}
