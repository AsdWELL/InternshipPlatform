using FluentValidation;
using InternshipPlatform.Application.Dtos.StudentGroup;
using System.Text.RegularExpressions;

namespace InternshipPlatform.Application.Validators.StudentGroup
{
    public class CreateStudentGroupValidator : AbstractValidator<CreateStudentGroupRequest>
    {
        private const int MaxStudyYears = 10;
        private const int MinStudyYears = 2;

        public CreateStudentGroupValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Название группы обязательно.")
                .Matches(@"^\d{2}[А-Яа-я]+[1-9]\d*$")
                .WithMessage("Название группы должно иметь формат: год поступления + код специализации + номер группы. Например: 22ВП1, 23ПСИ2.");

            RuleFor(x => x.Specialization)
                .NotEmpty()
                .WithMessage("Специализация обязательна.")
                .Matches(@"^[А-Яа-яЁё]+(?:\s+[А-Яа-яЁё]+)*$")
                .WithMessage("Специализация должна состоять только из русских букв и пробелов.");

            RuleFor(x => x.EnrollmentYear)
                .InclusiveBetween(DateTime.Now.Year - MaxStudyYears, DateTime.Now.Year)
                .WithMessage($"Год поступления должен быть от {DateTime.Now.Year - MaxStudyYears} до {DateTime.Now.Year}.");

            RuleFor(x => x.GraduationYear)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(x => x.EnrollmentYear)
                .WithMessage("Год выпуска должен быть больше года поступления.")
                .Must((request, graduationYear) =>
                    graduationYear - request.EnrollmentYear >= MinStudyYears)
                .WithMessage($"Год выпуска должен быть как минимум через {MinStudyYears} года после поступления.");

            RuleFor(x => x)
                .Must(IsGroupNameConsistentWithEnrollmentYear)
                .WithMessage("Первые две цифры в названии группы должны соответствовать году поступления.");
        }

        private static bool IsGroupNameConsistentWithEnrollmentYear(CreateStudentGroupRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return false;

            if (int.TryParse(request.Name[0..2], out var yearFromGroupName))
            {
                var enrollmentYearLastTwoDigits = request.EnrollmentYear % 100;

                return yearFromGroupName == enrollmentYearLastTwoDigits;
            }

            return false;
        }
    }
}
