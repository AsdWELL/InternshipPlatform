using FluentValidation;
using InternshipPlatform.Application.Dtos.Pagination;

namespace InternshipPlatform.Application.Validators
{
    public class PageRequestParametersValidator<T> : AbstractValidator<T> 
        where T : PageRequestParameters
    {
        public PageRequestParametersValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Указан отрицательный индекс страницы");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Указан отрицательный размер страницы");
        }
    }
}
