using FluentValidation;
using InternshipPlatform.Application.Dtos.User;

namespace InternshipPlatform.Application.Validators
{
    public class RegisterEmployerValidator : AbstractValidator<RegisterEmployerRequest>
    {
        public RegisterEmployerValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Поле Email является обязательным")
                .EmailAddress().WithMessage("Неверный формат электронной почты");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Поле Пароль является обязательным")
                .MinimumLength(8).WithMessage("Длина пароля не менее 8 символов");

            RuleFor(x => x.PasswordConfirm)
                .NotEmpty().WithMessage("Повторите пароль")
                .Equal(x => x.Password).WithMessage("Пароли не совпадают");
        }
    }
}
