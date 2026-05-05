using DbUp;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Infrastructure.Migration;
using InternshipPlatform.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InternshipPlatform.Infrastructure.Extensions
{
    public static class DatabaseExtension
    {
        private const string DatabaseConnectionStringName = "InternshipPlatformDB";

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(DatabaseConnectionStringName);

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Отсутствует строка подключения для БД");

            services
                .AddDbContext<InternshipPlatformContext>(options => options.UseNpgsql(connectionString))
                .MigrateDatabase(connectionString)
                .AddScoped<DbSeeder>();

            return services;
        }

        private static IServiceCollection MigrateDatabase(this IServiceCollection services, string connectionString)
        {
            EnsureDatabase.For.PostgresqlDatabase(connectionString);

            var upgrader = DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .WithTransaction()
                .WithVariablesDisabled()
                .LogToConsole()
                .Build();

            if (upgrader.IsUpgradeRequired())
            {
                upgrader.PerformUpgrade();
            }

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IStudentProfileRepository, StudentProfileRepository>()
                .AddScoped<IEmployerProfileRepository, EmployerProfileRepository>()
                .AddScoped<ICompanyRepository, CompanyRepository>()
                .AddScoped<ICuratorRepository, CuratorRepository>()
                .AddScoped<ISkillRepository, SkillRepository>()
                .AddScoped<ISpecializationRepository, SpecializationRepository>()
                .AddScoped<IResumeRepository, ResumeRepository>()
                .AddScoped<IVacancyRepository, VacancyRepository>()
                .AddScoped<IJobApplicationRepository, JobApplicationRepository>()
                .AddScoped<IChatRepository, ChatRepository>()
                .AddScoped<IMessageRepository, MessageRepository>()
                .AddScoped<IFavoriteVacancyRepository, FavoriteVacancyRepository>()
                .AddScoped<IResumeViewRepository, ResumeViewRepository>()
                .AddScoped<IVacancyViewRepository, VacancyViewRepository>()
                .AddScoped<IUniversityRepository, UniversityRepository>()
                .AddScoped<IStudentGroupRepository, StudentGroupRepository>()
                .AddScoped<IStudentGroupApplicationRepository, StudentGroupApplicationRepository>()
                .AddScoped<ICuratorGroupStatisticsRepository, CuratorGroupStatisticsRepository>()
                .AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
