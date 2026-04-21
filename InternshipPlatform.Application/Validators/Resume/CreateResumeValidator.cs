using FluentValidation;
using InternshipPlatform.Application.Dtos.Resume;

namespace InternshipPlatform.Application.Validators.Resume
{
    public class CreateResumeValidator : AbstractValidator<CreateResumeRequest>
    {
        public CreateResumeValidator()
        {
            RuleFor(x => x.SpecializationId)
                .GreaterThan(0)
                .WithMessage("Укажите корректную специализацию резюме");

            RuleFor(x => x.DesiredSalary)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Желаемая з/п должна быть больше или равна 0");

            RuleFor(x => x.SkillIds)
                .ForEach(skill => 
                    skill.GreaterThan(0)
                    .WithMessage("Укажите корректный навык"))
                .When(x => x.SkillIds?.Count > 0);
        }
    }
}
