using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.PracticeSubmission
{
    public class PracticeSubmissionCannotBeUpdatedException()
        : BadRequestException("Отчетность нельзя изменить после принятия работодателем или выставления оценки");
}
