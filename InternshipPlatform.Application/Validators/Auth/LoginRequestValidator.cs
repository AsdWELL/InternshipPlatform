using FluentValidation;
using InternshipPlatform.Application.Dtos.User;

namespace InternshipPlatform.Application.Validators.Auth
{
    public class LoginRequestValidator : AbstractValidator<LoginUserRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Поле Email является обязательным")
                .EmailAddress().WithMessage("Неверный формат электронной почты");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Поле Пароль является обязательным")
                .MinimumLength(8).WithMessage("Длина пароля не менее 8 символов");
        }
    }
}
