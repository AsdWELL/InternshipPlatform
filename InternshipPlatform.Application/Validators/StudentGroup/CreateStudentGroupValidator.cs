using FluentValidation;
using InternshipPlatform.Application.Dtos.StudentGroup;

namespace InternshipPlatform.Application.Validators.StudentGroup
{
    public class CreateStudentGroupValidator : AbstractValidator<CreateStudentGroupRequest>
    {
        private const int MaxStudyYears = 10;

        public CreateStudentGroupValidator()
        {
            RuleFor(x => x.EnrollmentYear)
                .InclusiveBetween(DateTime.Now.Year - MaxStudyYears, DateTime.Now.Year)
                .WithMessage($"Год поступления должен быть от {DateTime.Now.Year - MaxStudyYears} до {DateTime.Now.Year}.");

            RuleFor(x => x.EducationalProgramId)
                .GreaterThan(0)
                .WithMessage("Укажите корректную образовательную программу");
        }
    }
}
