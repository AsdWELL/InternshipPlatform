using FluentValidation;
using InternshipPlatform.Application.Dtos.Vacancy;

namespace InternshipPlatform.Application.Validators.Vacancy
{
    public class CreateVacancyValidator : AbstractValidator<CreateVacancyRequest>
    {
        public CreateVacancyValidator()
        {
            RuleFor(x => x.SalaryFrom)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Не может быть меньше 0");

            RuleFor(x => x.SalaryTo)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Не может быть меньше 0");

            RuleFor(x => x)
                .Must(x => x.SalaryFrom <= x.SalaryTo)
                .WithMessage("Левая граница з/п должна быть меньше или равна правой")
                .When(x => x.SalaryTo != 0 && x.SalaryFrom != 0);

            RuleFor(x => x.MinWorkExperienceYears)
                .GreaterThanOrEqualTo(0)
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
