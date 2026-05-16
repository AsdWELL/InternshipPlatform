using FluentValidation;
using InternshipPlatform.Application.Dtos.PracticeOffer;

namespace InternshipPlatform.Application.Validators.PracticeOffer
{
    public class UpdatePracticeOfferValidator : AbstractValidator<UpdatePracticeOfferRequest>
    {
        public UpdatePracticeOfferValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Заголовок предложения практики не может быть пустой строкой")
                .When(x => x.Title is not null);

            RuleFor(x => x.MaxStudents)
                .GreaterThan(0)
                .When(x => x.MaxStudents.HasValue)
                .WithMessage("Количество мест должно быть больше 0");

            RuleFor(x => x.SpecializationId)
                .GreaterThan(0)
                .When(x => x.SpecializationId.HasValue)
                .WithMessage("Укажите корректную специализацию предложения практики");
        }
    }
}
