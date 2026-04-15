using FluentValidation;
using InternshipPlatform.Application.Dtos.Vacancy;

namespace InternshipPlatform.Application.Validators.Vacancy
{
    public class UpdateVacancyValidator : AbstractValidator<UpdateVacancyRequest>
    {
        public UpdateVacancyValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Заголовок вакансии не может быть пустой строкой")
                .When(x => x.Title is not null);

            RuleFor(x => x.SalaryFrom)
                .GreaterThanOrEqualTo(0)
                .When(x => x.SalaryFrom.HasValue)
                .WithMessage("Не может быть меньше 0");

            RuleFor(x => x.SalaryTo)
                .GreaterThanOrEqualTo(0)
                .When(x => x.SalaryTo.HasValue)
                .WithMessage("Не может быть меньше 0");

            RuleFor(x => x)
                .Must(x => !x.SalaryFrom.HasValue || !x.SalaryTo.HasValue || x.SalaryFrom.Value <= x.SalaryTo.Value)
                .WithMessage("Левая граница з/п должна быть меньше правой");

            RuleFor(x => x.MinWorkExperienceYears)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MinWorkExperienceYears.HasValue)
                .WithMessage("Не может быть меньше 0");

            RuleFor(x => x.SpecializationId)
                .GreaterThan(0)
                .WithMessage("Укажите корректную специализацию вакансии");

            RuleFor(x => x.SkillIds)
                .ForEach(skill =>
                    skill.GreaterThan(0)
                    .WithMessage("Укажите корректный навык"))
                .When(x => x.SkillIds?.Count > 0);
        }
    }
}
