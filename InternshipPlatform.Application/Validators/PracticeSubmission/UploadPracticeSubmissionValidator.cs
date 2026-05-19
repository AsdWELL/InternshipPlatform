using FluentValidation;
using InternshipPlatform.Application.Dtos.PracticeSubmission;

namespace InternshipPlatform.Application.Validators.PracticeSubmission
{
    public class UploadPracticeSubmissionValidator : AbstractValidator<UploadPracticeSubmissionRequest>
    {
        private bool ValidUrl(string url)
        {
            if (url is null)
                return true;

            if (string.IsNullOrWhiteSpace(url))
                return true;

            if (Uri.TryCreate(url, UriKind.Absolute, out Uri? uri) &&
                    (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                return true;

            return false;
        }

        public UploadPracticeSubmissionValidator()
        {
            RuleFor(x => x.ReportFile!.Length)
                .GreaterThan(0)
                .When(x => x.ReportFile is not null)
                .WithMessage("Файл отчета не должен быть пустым");

            RuleFor(x => x.SolutionFile!.Length)
                .GreaterThan(0)
                .When(x => x.SolutionFile is not null)
                .WithMessage("Архив с решением не должен быть пустым");

            RuleFor(x => x.SolutionUrl)
                .Must(x => ValidUrl(x!))
                .WithMessage("Укажите корректную ссылку на решение")
                .When(x => x.SolutionUrl is not null);

            RuleFor(x => x)
                .Must(x => x.ReportFile is not null || x.SolutionFile is not null || !string.IsNullOrWhiteSpace(x.SolutionUrl))
                .WithMessage("Добавьте хоты бы один файл или ссылку");
        }
    }
}
