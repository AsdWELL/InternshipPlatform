using FluentValidation;
using InternshipPlatform.Application.Dtos.StudentProfile;

namespace InternshipPlatform.Application.Validators.StudentProfile
{
    public class UpdateStudentProfileValidator : AbstractValidator<UpdateStudentProfileRequest>
    {
        private const int MaxStudyDurationYears = 10;
        
        private const int MinUserAge = 14;
        private const int MaxUserAge = 80;
        
        private bool ValidGraduationYear(int year)
        {
            var currentYear = DateTime.Now.Year;
            
            return year >= currentYear && year <= currentYear + MaxStudyDurationYears;
        }

        private static bool RealisticStudentBirthdayDate(DateOnly? date)
        {
            if (date is null)
                return false;

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (date > today)
                return false;

            var age = today.Year - date.Value.Year;
            if (date.Value > today.AddYears(-age))
                age--;

            return age is >= MinUserAge and <= MaxUserAge;
        }

        private bool ValidOptionalUrl(string? value, params string[] allowedHosts)
        {
            if (value is null)
                return true;

            if (string.IsNullOrWhiteSpace(value))
                return true;

            if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
                return false;

            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
                return false;

            if (!allowedHosts.Contains(uri.Host, StringComparer.OrdinalIgnoreCase))
                return false;

            var path = uri.AbsolutePath.Trim('/');

            return !string.IsNullOrWhiteSpace(path);
        }

        public UpdateStudentProfileValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Имя не может быть пустой строкой")
                .Matches(@"^[а-яА-Я]+$").WithMessage("Поле Имя должно содержать только русские буквы")
                .When(x => x.Name is not null);

            RuleFor(x => x.Surname)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Фамилия не может быть пустой строкой")
                .Matches(@"^[а-яА-Я]+$").WithMessage("Поле Фамилия должно содержать только русские буквы")
                .When(x => x.Surname is not null);

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

            RuleFor(x => x.Patronymic)
                .Matches(@"^[а-яА-Я]+$").WithMessage("Поле Отчество должно содержать только русские буквы")
                .When(x => !string.IsNullOrWhiteSpace(x.Patronymic));

            RuleFor(x => x.BirthdayDate)
                .Cascade(CascadeMode.Stop)
                .Must(RealisticStudentBirthdayDate)
                .WithMessage($"Укажите корректную дату рождения. Возраст должен быть от {MinUserAge} до {MaxUserAge}")
                .When(x => x.BirthdayDate is not null);

            RuleFor(x => x.Phone)
                .Matches(@"^(\+7|7|8)\d{10}$").WithMessage("Неверный формат телефона");

            RuleFor(x => x.VkLink)
                .Must(x => ValidOptionalUrl(x, "vk.com", "www.vk.com"))
                .WithMessage("Укажите корректную ссылку на профиль VK")
                .When(x => x.VkLink is not null);

            RuleFor(x => x.GithubLink)
                .Must(x => ValidOptionalUrl(x, "github.com", "www.github.com"))
                .WithMessage("Укажите корректную ссылку на профиль GitHub")
                .When(x => x.GithubLink is not null);

            RuleFor(x => x.TgLink)
                .Must(x => ValidOptionalUrl(x, "t.me", "telegram.me"))
                .WithMessage("Укажите корректную ссылку на профиль Telegram")
                .When(x => x.TgLink is not null);

            RuleFor(x => x.MaxLink)
                .Must(x => ValidOptionalUrl(x, "max.ru", "www.max.ru", "web.max.ru"))
                .WithMessage("Укажите корректную ссылку на профиль MAX")
                .When(x => x.MaxLink is not null);
        }
    }
}
