using FluentValidation;
using InternshipPlatform.Application.Dtos.Resume;

namespace InternshipPlatform.Application.Validators.Resume
{
    public class UpdateWorkExperienceValidator : AbstractValidator<UpdateWorkExperienceRequest>
    {
        public UpdateWorkExperienceValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Не выбран опыт работы для редактирования");

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Название компании не может быть пустой строкой")
                .When(x => x.CompanyName is not null);

            RuleFor(x => x.Profession)
                .NotEmpty().WithMessage("Название професии не может быть пустой строкой")
                .When(x => x.Profession is not null);

            RuleFor(x => x.EndDateWork)
                .GreaterThan(x => x.StartDateWork)
                .WithMessage("Дата окончания работы должна быть позже даты начала")
                .When(x => x.EndDateWork is not null && x.StartDateWork is not null);
        }
    }
}
