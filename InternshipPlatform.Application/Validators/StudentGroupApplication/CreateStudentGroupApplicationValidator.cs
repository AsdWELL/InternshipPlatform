using FluentValidation;
using InternshipPlatform.Application.Dtos.StudentGroupApplication;

namespace InternshipPlatform.Application.Validators.StudentGroupApplication
{
    public class CreateStudentGroupApplicationValidator : AbstractValidator<CreateStudentGroupApplicationRequest>
    {
        public CreateStudentGroupApplicationValidator()
        {
            RuleFor(x => x.InviteCode)
                .NotEmpty().WithMessage("Укажите код приглашения в группу")
                .Matches(@"^[a-zA-Z0-9]{4}-[a-zA-Z0-9]{4}$")
                .WithMessage("Код приглашения должен быть в формате XXXX-XXXX и содержать только английские буквы и цифры");
        }
    }
}
