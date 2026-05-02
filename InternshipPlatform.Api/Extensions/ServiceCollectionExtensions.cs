using FluentValidation;
using InternshipPlatform.Application.Dtos.Company;
using InternshipPlatform.Application.Dtos.EmployerProflie;
using InternshipPlatform.Application.Dtos.JobApplication;
using InternshipPlatform.Application.Dtos.Kafka;
using InternshipPlatform.Application.Dtos.Resume;
using InternshipPlatform.Application.Dtos.StudentProfile;
using InternshipPlatform.Application.Dtos.User;
using InternshipPlatform.Application.Dtos.Vacancy;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Interfaces.Services.Auth;
using InternshipPlatform.Application.Services;
using InternshipPlatform.Application.Validators.Auth;
using InternshipPlatform.Application.Validators.EmployerProfile;
using InternshipPlatform.Application.Validators.JobApplication;
using InternshipPlatform.Application.Validators.Resume;
using InternshipPlatform.Application.Validators.StudentProfile;
using InternshipPlatform.Application.Validators.Vacancy;
using InternshipPlatform.Infrastructure.Services;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Filters;

namespace InternshipPlatform.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddEndpointsApiExplorer()
                            .AddSwaggerGen(options =>
                            {
                                options.SwaggerDoc("InternshipPlatform", new OpenApiInfo
                                {
                                    Version = "v1",
                                    Title = "Информационная платформа стажировок",
                                    Description = $"Годов, Поршнев"
                                });
                                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                            });
        }

        public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
        {
            var kafkaSection = configuration.GetRequiredSection("Kafka");
            services.Configure<KafkaSettings>(kafkaSection);

            return services.AddSingleton<NotificationProducer>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IImageService, ImageService>()
                .AddScoped<IAuthService, AuthService>()
                .AddScoped<IStudentProfileService, StudentProfileService>()
                .AddScoped<ICompanyService, CompanyService>()
                .AddScoped<IEmployerProfileService, EmployerProflieService>()
                .AddScoped<ISkillService, SkillService>()
                .AddScoped<ISpecializationService, SpecializationService>()
                .AddScoped<IResumeService, ResumeService>()
                .AddScoped<IVacancyService, VacancyService>()
                .AddScoped<IJobApplicationService, JobApplicationService>()
                .AddScoped<IChatService, ChatService>()
                .AddScoped<IMessageService, MessageService>()
                .AddScoped<IFavoriteVacancyService, FavoriteVacancyService>()
                .AddScoped<IResumeViewService, ResumeViewService>()
                .AddScoped<IVacancyViewService, VacancyViewService>()
                .AddScoped<IUniversityService, UniversityService>();
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services
                .AddTransient<IValidator<LoginUserRequest>, LoginRequestValidator>()
                .AddTransient<IValidator<RegisterStudentRequest>, RegisterStudentRequestValidator>()
                .AddTransient<IValidator<RegisterCompanyRequest>, RegisterCompanyValidator>()
                .AddTransient<IValidator<UpdateCompanyRequest>, UpdateCompanyValidator>()
                .AddTransient<IValidator<UpdateStudentProfileRequest>, UpdateStudentProfileValidator>()
                .AddTransient<IValidator<UpdateEmployerProfileRequest>, UpdateEmployerProflieValidator>()
                
                .AddTransient<IValidator<CreateResumeRequest>, CreateResumeValidator>()
                .AddTransient<IValidator<UpdateResumeRequest>, UpdateResumeValidator>()
                .AddTransient<IValidator<AddWorkExperienceRequest>, AddWorkExperienceValidator>()
                .AddTransient<IValidator<UpdateWorkExperienceRequest>, UpdateWorkExperienceValidator>()
                .AddTransient<IValidator<SearchResumeParameters>, SearchResumeParametersValidator>()
                
                .AddTransient<IValidator<CreateVacancyRequest>, CreateVacancyValidator>()
                .AddTransient<IValidator<UpdateVacancyRequest>, UpdateVacancyValidator>()
                .AddTransient<IValidator<SearchVacancyParameters>, SearchVacancyValidator>()
                
                .AddTransient<IValidator<GetStudentApplicationsParameters>, GetStudentApplicationsValidator>()
                .AddTransient<IValidator<GetEmployerApplicationsParameters>, GetEmployerApplicationsValidator>();
        }
    }
}
