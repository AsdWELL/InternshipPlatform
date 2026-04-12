using Bogus;
using InternshipPlatform.Application.Dtos.Auth;
using InternshipPlatform.Application.Interfaces.Services.Auth;
using InternshipPlatform.Application.Values;
using InternshipPlatform.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InternshipPlatform.Infrastructure.Migration
{
    public class DbSeeder(
        InternshipPlatformContext context,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IOptions<TokenOptions> tokenOptions,
        ILogger<DbSeeder> logger)
    {
        private const string BasePassword = "qwerty123";
        private const int RandomizerSeed = 8675309;

        private const int UsersAmount = 10000;

        private const int MinResumesPerStudent = 2;
        private const int MaxResumesPerStudent = 5;

        private const int MinSkills = 3;
        private const int MaxSkills = 8;

        private const int WorkExperienceChancePercent = 50;
        private const int MinWorkExperiencesPerResume = 1;
        private const int MaxWorkExperiencesPerResume = 3;

        private const int MinVacanciesPerCompany = 2;
        private const int MaxVacanciesPerCompany = 8;

        private TokenOptions TokenOptionsValue => tokenOptions.Value;

        private void GenerateStudents(List<User> users)
        {
            var universities = new[]
            {
                "ПГУ", "ПГУАС", "ПензГТУ", "ПГАУ", "БГУ", "БНТУ", "БГУИР", "БГЭУ", "МГЛУ", "БГМУ", "ГрГУ им. Янки Купалы"
            };

            var specializations = new[]
            {
                "Программная инженерия",
                "Информатика и технологии программирования",
                "Информационные системы и технологии",
                "Прикладная информатика",
                "Искусственный интеллект",
                "Кибербезопасность",
                "Бизнес-аналитика"
            };

            var students = users
                .Where(u => u.Role.Name == Roles.Student)
                .ToList();

            var studentProfileFaker = new Faker<StudentProfile>("ru")
                .RuleFor(p => p.Name, f => f.Name.FirstName())
                .RuleFor(p => p.Surname, f => f.Name.LastName())
                .RuleFor(p => p.Patronymic, f => f.Random.Bool(0.7f) ? f.Name.FirstName() + "ович" : null)
                .RuleFor(p => p.BirthdayDate, f =>
                {
                    var birthDate = f.Date.BetweenDateOnly(
                        DateOnly.FromDateTime(DateTime.Today.AddYears(-30)),
                        DateOnly.FromDateTime(DateTime.Today.AddYears(-17)));

                    return birthDate;
                })
                .RuleFor(p => p.Phone, f => f.Phone.PhoneNumber("+79#########"))
                .RuleFor(p => p.VkLink, f => f.Random.Bool(0.5f) ? $"https://vk.com/{f.Internet.UserName()}" : null)
                .RuleFor(p => p.TgLink, f => f.Random.Bool(0.7f) ? $"https://t.me/{f.Internet.UserName()}" : null)
                .RuleFor(p => p.MaxLink, f => f.Random.Bool(0.2f) ? $"https://max.ru/{f.Internet.UserName()}" : null)
                .RuleFor(p => p.GithubLink, f => f.Random.Bool(0.6f) ? $"https://github.com/{f.Internet.UserName()}" : null)
                .RuleFor(p => p.University, f => f.PickRandom(universities))
                .RuleFor(p => p.Specialization, f => f.PickRandom(specializations))
                .RuleFor(p => p.GraduationYear, f => f.Date.Future(5).Year)
                .RuleFor(p => p.AvatarPath, _ => null);

            var studentProfiles = students
                .Select(user =>
                {
                    var profile = studentProfileFaker.Generate();
                    profile.UserId = user.Id;
                    return profile;
                })
                .ToList();

            context.StudentProfiles.AddRange(studentProfiles);
            context.SaveChanges();
        }

        private void GenerateCompaniesAndEmployers(List<User> users)
        {
            var companyPrefixes = new[]
            {
                "ООО", "ЗАО", "ЧУП"
            };

            var companyDomains = new[]
            {
                "tech", "soft", "digital", "labs", "group", "systems", "dev"
            };

            var employers = users
                .Where(u => u.Role.Name == Roles.Employer)
                .ToList();

            var companyFaker = new Faker<Company>("ru")
                .RuleFor(c => c.Name, f =>
                {
                    var prefix = f.PickRandom(companyPrefixes);
                    var companyName = f.Company.CompanyName();
                    return $"{prefix} \"{companyName}\"";
                })
                .RuleFor(c => c.Inn, f => f.Random.ReplaceNumbers("#########"))
                .RuleFor(c => c.Link, f =>
                {
                    var name = f.Company.CompanyName().ToLowerInvariant()
                        .Replace(" ", "")
                        .Replace("\"", "")
                        .Replace("'", "");

                    var domain = f.PickRandom(companyDomains);
                    return $"https://{name}.{domain}";
                })
                .RuleFor(c => c.Description, f => f.Company.CatchPhrase())
                .RuleFor(c => c.LogoPath, _ => null);

            var companies = employers
                .Select(_ => companyFaker.Generate())
                .ToList();

            context.Companies.AddRange(companies);
            context.SaveChanges();

            var employerProfiles = new List<EmployerProfile>(employers.Count);

            for (int i = 0; i < employers.Count; i++)
                employerProfiles.Add(new EmployerProfile
                {
                    UserId = employers[i].Id,
                    CompanyId = companies[i].Id
                });

            context.EmployerProfiles.AddRange(employerProfiles);
            context.SaveChanges();
        } 

        private void GenerateUsers()
        {
            if (context.Users.Any())
                return;

            var passwordHash = passwordHasher.Generate(BasePassword);
            var refreshTokenExpiredAt = DateTime.UtcNow.AddHours(TokenOptionsValue.ExpiresAfterHours);
            var roles = context.Roles.ToList();

            Randomizer.Seed = new Random(RandomizerSeed);

            var count = 1;

            var userFaker = new Faker<User>("ru")
                .RuleFor(u => u.Role, f => f.PickRandom(roles))
                .RuleFor(u => u.Email, (f, u) => $"{u.Role.Name.ToLower()}_{count++}@mail.ru")
                .RuleFor(u => u.PasswordHash, _ => passwordHash)
                .RuleFor(u => u.RefreshToken, _ => tokenService.GenerateRefreshToken())
                .RuleFor(u => u.RefreshTokenExpiredAt, _ => refreshTokenExpiredAt)
                .RuleFor(u => u.IsVerified, _ => false);

            var users = userFaker.Generate(UsersAmount);

            context.Users.AddRange(users);
            context.SaveChanges();

            GenerateStudents(users);

            GenerateCompaniesAndEmployers(users);
        }

        private void GenerateResumes()
        {
            if (context.Resumes.Any())
                return;

            var students = context.StudentProfiles.ToList();
            if (students.Count == 0)
                return;

            var skills = context.Skills.ToList();
            var specializations = context.Specializations.ToList();

            if (skills.Count == 0 || specializations.Count == 0)
                return;

            Randomizer.Seed = new Random(RandomizerSeed);

            var resumeDescriptions = new[]
            {
                "Студент, заинтересованный в прохождении стажировки и развитии практических навыков в команде разработки.",
                "Быстро обучаюсь, умею работать с документацией и хочу применять знания на реальных проектах.",
                "Ищу возможность получить коммерческий опыт, развиваться под руководством наставника и приносить пользу команде.",
                "Интересуюсь backend-разработкой, проектированием API и работой с базами данных.",
                "Хочу развиваться в frontend-разработке, создавать удобные интерфейсы и улучшать пользовательский опыт.",
                "Интересуюсь тестированием, качеством ПО и автоматизацией проверок.",
                "Ищу стажировку для старта карьеры в IT и дальнейшего профессионального роста."
            };

            var workDescriptions = new[]
            {
                "Участвовал в разработке и поддержке внутренних сервисов компании.",
                "Занимался исправлением дефектов, доработкой функциональности и тестированием изменений.",
                "Работал с командой разработки над реализацией новых возможностей продукта.",
                "Подготавливал техническую документацию и помогал в сопровождении проекта.",
                "Выполнял задачи по разработке пользовательского интерфейса и интеграции с API.",
                "Помогал в анализе требований и реализации прикладной бизнес-логики."
            };

            var companyNames = new[]
            {
                "ООО \"ТехСофт\"",
                "ООО \"Диджитал Решения\"",
                "ЗАО \"ИнноТех\"",
                "ООО \"Прайм Дев\"",
                "ООО \"Веб Аналитика\"",
                "ЧУП \"Смарт Системс\"",
                "ООО \"Бизнес Автоматизация\""
            };

            var professionNames = new[]
            {
                "Стажёр-разработчик",
                "Junior .NET Developer",
                "Junior Frontend Developer",
                "QA Intern",
                "Техник-программист",
                "Стажёр-аналитик",
                "Стажёр-тестировщик"
            };

            var faker = new Faker("ru");

            var resumes = new List<Resume>();

            foreach (var student in students)
            {
                var resumesCount = faker.Random.Int(MinResumesPerStudent, MaxResumesPerStudent);

                for (int i = 0; i < resumesCount; i++)
                {
                    var specialization = faker.PickRandom(specializations);

                    var skillsCount = Math.Min(
                        faker.Random.Int(MinSkills, MaxSkills),
                        skills.Count);

                    var resumeSkills = faker.Random
                        .Shuffle(skills)
                        .Take(skillsCount)
                        .ToList();

                    var resume = new Resume
                    {
                        StudentId = student.UserId,
                        SpecializationId = specialization.Id,
                        LastUpdateDate = DateOnly.FromDateTime(
                            faker.Date.Between(
                                DateTime.UtcNow.AddMonths(-6),
                                DateTime.UtcNow)),
                        Description = faker.PickRandom(resumeDescriptions),
                        IsActive = faker.Random.Bool(0.8f),
                        Region = faker.Address.City(),
                        DesiredSalary = faker.Random.Bool(0.85f)
                            ? faker.Random.Int(2000, 20000) * 10
                            : null,
                        Skills = resumeSkills,
                        WorkExperiences = []
                    };

                    bool addWorkExperience = faker.Random.Int(1, 100) <= WorkExperienceChancePercent;

                    if (addWorkExperience)
                    {
                        var workExperiencesCount = faker.Random.Int(
                            MinWorkExperiencesPerResume,
                            MaxWorkExperiencesPerResume);

                        for (int j = 0; j < workExperiencesCount; j++)
                        {
                            var startDate = faker.Date.BetweenDateOnly(
                                DateOnly.FromDateTime(DateTime.Today.AddYears(-5)),
                                DateOnly.FromDateTime(DateTime.Today.AddMonths(-3)));

                            var startDateTime = startDate.ToDateTime(TimeOnly.MinValue);

                            DateOnly? endDate = faker.Date.BetweenDateOnly(
                                DateOnly.FromDateTime(startDateTime.AddMonths(1)),
                                DateOnly.FromDateTime(DateTime.Today));

                            resume.WorkExperiences.Add(new WorkExperience
                            {
                                CompanyName = faker.PickRandom(companyNames),
                                Profession = faker.PickRandom(professionNames),
                                StartDateWork = startDate,
                                EndDateWork = endDate,
                                WorkDescription = faker.PickRandom(workDescriptions)
                            });
                        }
                    }

                    resumes.Add(resume);
                }
            }

            context.Resumes.AddRange(resumes);
            context.SaveChanges();
        }

        private void GenerateVacancies()
        {
            if (context.Vacancies.Any())
                return;

            var companies = context.Companies.ToList();
            var skills = context.Skills.ToList();
            var specializations = context.Specializations.ToList();

            if (companies.Count == 0 || specializations.Count == 0 || skills.Count == 0)
                return;

            Randomizer.Seed = new Random(RandomizerSeed);

            var vacancyTitles = new[]
            {
                "Стажёр-разработчик .NET",
                "Стажёр backend-разработчик",
                "Стажёр frontend-разработчик",
                "Стажёр fullstack-разработчик",
                "Junior .NET Developer",
                "Junior Frontend Developer",
                "Junior Backend Developer",
                "Стажёр QA Engineer",
                "Стажёр тестировщик",
                "Стажёр бизнес-аналитик",
                "Стажёр системный аналитик",
                "Стажёр DevOps Engineer",
                "Стажёр Data Analyst",
                "Стажёр Python Developer"
            };

            var vacancyDescriptions = new[]
            {
                "Приглашаем студента или начинающего специалиста для участия в коммерческих и внутренних проектах компании. Предусмотрено наставничество, обучение и постепенное погружение в рабочие задачи.",
                "Ищем стажёра в команду разработки. Нужно будет помогать в реализации нового функционала, исправлении ошибок и сопровождении существующего решения.",
                "Подойдёт кандидатам, которые хотят получить первый практический опыт, развиваться в команде и работать с современным стеком технологий.",
                "В рамках стажировки предстоит взаимодействовать с командой, участвовать в код-ревью, изучать внутренние процессы разработки и выполнять реальные задачи проекта.",
                "Отличная возможность начать карьеру в IT, получить опыт командной работы и прокачать профессиональные навыки под руководством опытных специалистов."
            };

            var faker = new Faker("ru");
            var vacancies = new List<Vacancy>();

            foreach (var company in companies)
            {
                var vacanciesCount = faker.Random.Int(MinVacanciesPerCompany, MaxVacanciesPerCompany);

                for (int i = 0; i < vacanciesCount; i++)
                {
                    var specialization = faker.PickRandom(specializations);

                    int salaryFrom = faker.Random.Int(2000, 20000) * 10;
                    int salaryTo = salaryFrom + faker.Random.Int(20, 150) * 10;

                    bool salaryHidden = faker.Random.Bool(0.15f);
                    bool isRemote = faker.Random.Bool(0.35f);

                    var skillsCount = Math.Min(
                        faker.Random.Int(MinSkills, MaxSkills),
                        skills.Count);

                    var vacancySkills = faker.Random
                        .Shuffle(skills)
                        .Take(skillsCount)
                        .ToList();

                    vacancies.Add(new Vacancy
                    {
                        Title = faker.PickRandom(vacancyTitles),
                        Description = faker.PickRandom(vacancyDescriptions),
                        SalaryFrom = salaryHidden ? null : salaryFrom,
                        SalaryTo = salaryHidden ? null : salaryTo,
                        IsRemote = isRemote,
                        Region = isRemote ? null : faker.Address.City(),
                        IsActive = faker.Random.Bool(0.85f),
                        ViewsCount = faker.Random.Int(0, 5000),
                        MinWorkExperienceYears = faker.Random.Int(0, 3),
                        SpecializationId = specialization.Id,
                        CompanyId = company.Id,
                        Skills = vacancySkills
                    });
                }
            }

            context.Vacancies.AddRange(vacancies);
            context.SaveChanges();
        }

        public void Seed()
        {
            int totalTime = Environment.TickCount;

            int startTime = Environment.TickCount;
            GenerateUsers();
            logger.LogInformation($"Пользователи сгенерированы за {Environment.TickCount - startTime}мс");

            startTime = Environment.TickCount;
            GenerateResumes();
            logger.LogInformation($"Резюме студентов сгенерированы за {Environment.TickCount - startTime}мс");

            startTime = Environment.TickCount;
            GenerateVacancies();
            logger.LogInformation($"Вакансии комипаний сгенерированы за {Environment.TickCount - startTime}мс");

            logger.LogInformation($"Общее время генерации данных = {Environment.TickCount - totalTime}мс");
        }
    }
}
