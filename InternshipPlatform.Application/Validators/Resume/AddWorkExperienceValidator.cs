using FluentValidation;
using InternshipPlatform.Application.Dtos.Resume;

namespace InternshipPlatform.Application.Validators.Resume
{
    public class AddWorkExperienceValidator : AbstractValidator<AddWorkExperienceRequest>
    {
        public AddWorkExperienceValidator()
        {
            RuleFor(x => x.ResumeId)
                 .NotEmpty().WithMessage("Не выбрано резюме для редактирования");

            RuleFor(x => x.EndDateWork)
                .GreaterThan(x => x.StartDateWork)
                .WithMessage("Дата окончания работы должна быть позже даты начала")
                .When(x => x.EndDateWork is not null);
        }
    }
}
