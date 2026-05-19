using FluentValidation;
using InternshipPlatform.Application.Dtos.PracticeSubmission;

namespace InternshipPlatform.Application.Validators.PracticeSubmission
{
    public class GradePracticeSubmissionValidator : AbstractValidator<GradePracticeSubmissionRequest>
    {
        public GradePracticeSubmissionValidator()
        {
            RuleFor(x => x.Grade)
                .InclusiveBetween(2, 5)
                .WithMessage("Оценка должна быть в диапазоне от 2 до 5");
        }
    }
}
