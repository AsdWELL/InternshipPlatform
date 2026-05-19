using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.PracticeSubmission
{
    public class PracticeSubmissionCannotBeGradedException()
        : BadRequestException("Оценку можно выставить только после принятия отчетности работодателем");
}
