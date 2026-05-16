using FluentValidation;
using InternshipPlatform.Application.Dtos.PracticeOffer;

namespace InternshipPlatform.Application.Validators.PracticeOffer
{
    public class SearchPracticeOfferValidator : PageRequestParametersValidator<SearchPracticeOfferParameters>
    {
        public SearchPracticeOfferValidator()
        {
            RuleFor(x => x.SpecializationId)
                .GreaterThan(0)
                .WithMessage("Укажите корректную специализацию предложения практики")
                .When(x => x.SpecializationId.HasValue);
        }
    }
}
