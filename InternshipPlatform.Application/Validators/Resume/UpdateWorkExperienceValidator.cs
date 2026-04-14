using FluentValidation;
using InternshipPlatform.Application.Dtos.Resume;

namespace InternshipPlatform.Application.Validators.Resume
{
    public class UpdateWorkExperienceValidator : AbstractValidator<UpdateWorkExperienceRequest>
    {
        public UpdateWorkExperienceValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Название компании не может быть пустой строкой")
                .When(x => x.CompanyName is not null);

            RuleFor(x => x.Profession)
                .NotEmpty().WithMessage("Название професии не может быть пустой строкой")
                .When(x => x.Profession is not null);

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            RuleFor(x => x.StartDateWork)
                .LessThanOrEqualTo(today)
                .WithMessage("Дата начала работы не может быть больше текущей даты")
                .When(x => x.StartDateWork.HasValue);

            RuleFor(x => x.EndDateWork)
                .LessThanOrEqualTo(today)
                .WithMessage("Дата окончания работы не может быть больше текущей даты")
                .When(x => x.EndDateWork.HasValue);

            RuleFor(x => x.EndDateWork)
                .GreaterThan(x => x.StartDateWork)
                .WithMessage("Дата окончания работы должна быть позже даты начала")
                .When(x => x.EndDateWork is not null && x.StartDateWork is not null);
        }
    }
}
