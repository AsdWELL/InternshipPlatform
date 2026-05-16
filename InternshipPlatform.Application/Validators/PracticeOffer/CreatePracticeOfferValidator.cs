using FluentValidation;
using InternshipPlatform.Application.Dtos.PracticeOffer;

namespace InternshipPlatform.Application.Validators.PracticeOffer
{
    public class CreatePracticeOfferValidator : AbstractValidator<CreatePracticeOfferRequest>
    {
        public CreatePracticeOfferValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Заголовок предложения практики не может быть пустой строкой");

            RuleFor(x => x.MaxStudents)
                .GreaterThan(0)
                .WithMessage("Количество мест должно быть больше 0");

            RuleFor(x => x.SpecializationId)
                .GreaterThan(0)
                .WithMessage("Укажите корректную специализацию предложения практики");
        }
    }
}
