using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.PracticeSubmission
{
    public class PracticeSubmissionFileNotFoundException() : NotFoundException("Файл отчетности не найден");
}
