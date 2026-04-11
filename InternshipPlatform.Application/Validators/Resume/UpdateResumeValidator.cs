using FluentValidation;
using InternshipPlatform.Application.Dtos.Resume;

namespace InternshipPlatform.Application.Validators.Resume
{
    public class UpdateResumeValidator : AbstractValidator<UpdateResumeRequest>
    {
        public UpdateResumeValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Не выбрано резюме для редактирования");
            
            RuleFor(x => x.DesiredSalary)
                .Must(salary => salary > 0)
                .WithMessage("Желаемая з/п должна быть больше 0")
                .When(x => x.DesiredSalary.HasValue);
        }
    }
}
