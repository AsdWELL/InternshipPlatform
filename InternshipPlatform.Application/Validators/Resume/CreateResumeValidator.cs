using FluentValidation;
using InternshipPlatform.Application.Dtos.Resume;

namespace InternshipPlatform.Application.Validators.Resume
{
    public class CreateResumeValidator : AbstractValidator<CreateResumeRequest>
    {
        public CreateResumeValidator()
        {
            RuleFor(x => x.DesiredSalary)
                .Must(salary => salary > 0)
                .WithMessage("Желаемая з/п должна быть больше 0")
                .When(salary => salary is not null);
        }
    }
}
