using FluentValidation;
using InternshipPlatform.Application.Dtos.JobApplication;

namespace InternshipPlatform.Application.Validators.JobApplication
{
    public class GetEmployerApplicationsValidator : PageRequestParametersValidator<GetEmployerApplicationsParameters>
    {
        public GetEmployerApplicationsValidator() : base()
        {
            RuleFor(x => x.VacancyId)
                .GreaterThan(0)
                .WithMessage("Укажите корректную вакансию")
                .When(x => x.VacancyId.HasValue);
        }
    }
}
