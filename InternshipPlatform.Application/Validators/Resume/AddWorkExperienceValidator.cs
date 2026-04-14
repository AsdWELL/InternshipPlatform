using FluentValidation;
using InternshipPlatform.Application.Dtos.Resume;

namespace InternshipPlatform.Application.Validators.Resume
{
    public class AddWorkExperienceValidator : AbstractValidator<AddWorkExperienceRequest>
    {
        public AddWorkExperienceValidator()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            RuleFor(x => x.StartDateWork)
                .LessThanOrEqualTo(today)
                .WithMessage("Дата начала работы не может быть больше текущей даты");

            RuleFor(x => x.EndDateWork)
                .LessThanOrEqualTo(today)
                .WithMessage("Дата окончания работы не может быть больше текущей даты")
                .When(x => x.EndDateWork.HasValue);

            RuleFor(x => x.EndDateWork)
                .GreaterThan(x => x.StartDateWork)
                .WithMessage("Дата окончания работы должна быть позже даты начала")
                .When(x => x.EndDateWork.HasValue);
        }
    }
}
