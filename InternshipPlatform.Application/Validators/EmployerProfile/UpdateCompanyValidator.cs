using FluentValidation;
using InternshipPlatform.Application.Dtos.Company;

namespace InternshipPlatform.Application.Validators.EmployerProfile
{
    public class UpdateCompanyValidator : AbstractValidator<UpdateCompanyRequest>
    {
        private bool ValidUrl(string url)
        {
            if (url is null)
                return true;

            if (string.IsNullOrWhiteSpace(url))
                return false;

            if (Uri.TryCreate(url, UriKind.Absolute, out Uri? uri) &&
                    (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                return true;

            return false;
        }

        public UpdateCompanyValidator()
        {
            RuleFor(x => x.Link)
                .Must(x => ValidUrl(x!))
                .WithMessage("Укажите корректную ссылку")
                .When(x => x.Link is not null);
        }
    }
}
