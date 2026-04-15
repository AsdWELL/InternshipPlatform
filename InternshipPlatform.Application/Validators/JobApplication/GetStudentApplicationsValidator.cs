using FluentValidation;
using InternshipPlatform.Application.Dtos.JobApplication;

namespace InternshipPlatform.Application.Validators.JobApplication
{
    public class GetStudentApplicationsValidator : PageRequestParametersValidator<GetStudentApplicationsParameters>
    {
        public GetStudentApplicationsValidator() : base()
        {
            RuleFor(x => x.ResumeId)
                .GreaterThan(0)
                .WithMessage("Укажите корректное резюме")
                .When(x => x.ResumeId.HasValue);
        }
    }
}
