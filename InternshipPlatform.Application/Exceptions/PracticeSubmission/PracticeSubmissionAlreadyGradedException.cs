using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.PracticeSubmission
{
    public class PracticeSubmissionAlreadyGradedException()
        : BadRequestException("Оцененная отчетность не может быть отправлена на повторную проверку");
}
