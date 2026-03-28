using FluentValidation;
using InternshipPlatform.Application.Dtos.EmployerProflie;

namespace InternshipPlatform.Application.Validators.EmployerProfile
{
    public class UpdateEmployerProflieValidator : AbstractValidator<UpdateEmployerProflieRequest>
    {
        public UpdateEmployerProflieValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Почта не может быть пустой строкой")
                .EmailAddress().WithMessage("Неверный формат электронной почты")
                .When(x => x.Email is not null);

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Пароль не может быть пустой строкой")
                .MinimumLength(8).WithMessage("Длина пароля не менее 8 символов")
                .When(x => x.Password is not null);

            RuleFor(x => x.PasswordConfirm)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Повторите пароль")
                .Equal(x => x.Password).WithMessage("Пароли не совпадают")
                .When(x => x.Password is not null);
        }
    }
}
