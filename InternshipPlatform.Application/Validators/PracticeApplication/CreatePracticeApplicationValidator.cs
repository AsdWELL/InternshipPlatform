using FluentValidation;
using InternshipPlatform.Application.Dtos.PracticeApplication;

namespace InternshipPlatform.Application.Validators.PracticeApplication
{
    public class CreatePracticeApplicationValidator : AbstractValidator<CreatePracticeApplicationRequest>
    {
        public CreatePracticeApplicationValidator()
        {
            RuleFor(x => x.PracticeOfferId)
                .GreaterThan(0)
                .WithMessage("Укажите корректное предложение практики");
        }
    }
}
