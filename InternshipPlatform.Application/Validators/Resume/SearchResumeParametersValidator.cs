using FluentValidation;
using InternshipPlatform.Application.Dtos.Resume;

namespace InternshipPlatform.Application.Validators.Resume
{
    public class SearchResumeParametersValidator : PageRequestParametersValidator<SearchResumeParameters>
    {
        public SearchResumeParametersValidator() : base()
        {
            RuleFor(x => x.SalaryFrom)
                .GreaterThanOrEqualTo(0)
                .When(x => x.SalaryFrom.HasValue)
                .WithMessage("Не может быть меньше 0");

            RuleFor(x => x.SalaryTo)
                .GreaterThanOrEqualTo(0)
                .When(x => x.SalaryTo.HasValue)
                .WithMessage("Не может быть меньше 0");

            RuleFor(x => x.AgeFrom)
                .GreaterThanOrEqualTo(0)
                .When(x => x.AgeFrom.HasValue)
                .WithMessage("Не может быть меньше 0");

            RuleFor(x => x.AgeTo)
                .GreaterThanOrEqualTo(0)
                .When(x => x.AgeTo.HasValue)
                .WithMessage("Не может быть меньше 0");

            RuleFor(x => x)
                .Must(x => !x.SalaryFrom.HasValue || !x.SalaryTo.HasValue || x.SalaryFrom.Value <= x.SalaryTo.Value)
                .WithMessage("Левая граница з/п должна быть меньше правой");

            RuleFor(x => x)
                .Must(x => !x.AgeFrom.HasValue || !x.AgeTo.HasValue || x.AgeFrom.Value <= x.AgeTo.Value)
                .WithMessage("Левая граница возраста должна быть меньше правой");

            RuleFor(x => x)
                .Must(x => !x.UpdatedFrom.HasValue || !x.UpdatedTo.HasValue || x.UpdatedFrom.Value <= x.UpdatedTo.Value)
                .WithMessage("Левая граница интервала обновления должна быть меньше правой");
        }
    }
}
