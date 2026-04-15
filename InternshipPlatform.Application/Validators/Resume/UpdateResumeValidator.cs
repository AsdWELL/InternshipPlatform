using FluentValidation;
using InternshipPlatform.Application.Dtos.Resume;

namespace InternshipPlatform.Application.Validators.Resume
{
    public class UpdateResumeValidator : AbstractValidator<UpdateResumeRequest>
    {
        public UpdateResumeValidator()
        {
            RuleFor(x => x.SpecializationId)
                .GreaterThan(0)
                .WithMessage("Укажите корректную специализацию резюме")
                .When(x => x.SpecializationId.HasValue);

            RuleFor(x => x.DesiredSalary)
                .GreaterThan(0)
                .WithMessage("Желаемая з/п должна быть больше 0")
                .When(x => x.DesiredSalary.HasValue);

            RuleFor(x => x.SkillIds)
                .ForEach(skill =>
                    skill.GreaterThan(0)
                    .WithMessage("Укажите корректный навык"))
                .When(x => x.SkillIds?.Count > 0);
        }
    }
}
