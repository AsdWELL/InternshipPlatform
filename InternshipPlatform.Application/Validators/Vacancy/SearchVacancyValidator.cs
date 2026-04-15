using FluentValidation;
using InternshipPlatform.Application.Dtos.Vacancy;

namespace InternshipPlatform.Application.Validators.Vacancy
{
    public class SearchVacancyValidator : PageRequestParametersValidator<SearchVacancyParameters>
    {
        public SearchVacancyValidator()
        {
            RuleFor(x => x.SpecializationId)
                .GreaterThan(0)
                .WithMessage("Укажите корректную специализацию резюме")
                .When(x => x.SpecializationId.HasValue);

            RuleFor(x => x.SalaryFrom)
                .GreaterThanOrEqualTo(0)
                .When(x => x.SalaryFrom.HasValue)
                .WithMessage("Не может быть меньше 0");

            RuleFor(x => x.SalaryTo)
                .GreaterThanOrEqualTo(0)
                .When(x => x.SalaryTo.HasValue)
                .WithMessage("Не может быть меньше 0");

            RuleFor(x => x.MinWorkExperienceYears)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MinWorkExperienceYears.HasValue)
                .WithMessage("Не может быть меньше 0");

            RuleFor(x => x.MaxWorkExperienceYears)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MaxWorkExperienceYears.HasValue)
                .WithMessage("Не может быть меньше 0");

            RuleFor(x => x)
               .Must(x => !x.SalaryFrom.HasValue || !x.SalaryTo.HasValue || x.SalaryFrom.Value <= x.SalaryTo.Value)
               .WithMessage("Левая граница з/п должна быть меньше правой");

            RuleFor(x => x)
               .Must(x => !x.MinWorkExperienceYears.HasValue || !x.MaxWorkExperienceYears.HasValue || x.MinWorkExperienceYears.Value <= x.MaxWorkExperienceYears.Value)
               .WithMessage("Левая граница опыта должна быть меньше правой");
        }
    }
}
