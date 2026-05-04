using FluentValidation;
using InternshipPlatform.Application.Dtos.User;

namespace InternshipPlatform.Application.Validators.Auth
{
    public class RegisterStudentRequestValidator : AbstractValidator<RegisterStudentRequest>
    {
        public RegisterStudentRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Поле Имя является обязательным")
                .Matches(@"^[а-яА-Я]+$").WithMessage("Поле Имя должно содержать только русские буквы");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Поле Фамилия является обязательным")
                .Matches(@"^[а-яА-Я]+$").WithMessage("Поле Фамилия должно содержать только русские буквы");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Поле Email является обязательным")
                .EmailAddress().WithMessage("Неверный формат электронной почты");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Поле Пароль является обязательным")
                .MinimumLength(8).WithMessage("Длина пароля не менее 8 символов");

            RuleFor(x => x.PasswordConfirm)
                .NotEmpty().WithMessage("Повторите пароль")
                .Equal(x => x.Password).WithMessage("Пароли не совпадают");

            RuleFor(x => x.InviteCode)
                .Matches(@"^[a-zA-Z0-9]{4}-[a-zA-Z0-9]{4}$")
                .WithMessage("Код приглашения должен быть в формате XXXX-XXXX и содержать только английские буквы и цифры")
                .When(x => x.InviteCode is not null);
        }
    }
}
