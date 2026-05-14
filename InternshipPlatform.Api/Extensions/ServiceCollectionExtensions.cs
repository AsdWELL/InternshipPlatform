using FluentValidation;
using InternshipPlatform.Api.Authorization;
using InternshipPlatform.Api.Authorization.Handlers;
using InternshipPlatform.Api.Authorization.Requirements;
using InternshipPlatform.Application.Dtos.Company;
using InternshipPlatform.Application.Dtos.EmployerProflie;
using InternshipPlatform.Application.Dtos.JobApplication;
using InternshipPlatform.Application.Dtos.Kafka;
using InternshipPlatform.Application.Dtos.Resume;
using InternshipPlatform.Application.Dtos.StudentGroup;
using InternshipPlatform.Application.Dtos.StudentGroupApplication;
using InternshipPlatform.Application.Dtos.StudentProfile;
using InternshipPlatform.Application.Dtos.Teacher;
using InternshipPlatform.Application.Dtos.User;
using InternshipPlatform.Application.Dtos.Vacancy;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Interfaces.Services.Auth;
using InternshipPlatform.Application.Services;
using InternshipPlatform.Application.Validators.Auth;
using InternshipPlatform.Application.Validators.EmployerProfile;
using InternshipPlatform.Application.Validators.JobApplication;
using InternshipPlatform.Application.Validators.Resume;
using InternshipPlatform.Application.Validators.StudentGroup;
using InternshipPlatform.Application.Validators.StudentGroupApplication;
using InternshipPlatform.Application.Validators.StudentProfile;
using InternshipPlatform.Application.Validators.Teacher;
using InternshipPlatform.Application.Validators.Vacancy;
using InternshipPlatform.Application.Values;
using InternshipPlatform.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
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
                .AddScoped<ITeacherService, TeacherService>()
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
                .AddScoped<IUniversityService, UniversityService>()
                .AddScoped<IInviteCodeGenerator, InviteCodeGenerator>()
                .AddScoped<IStudentGroupService, StudentGroupService>()
                .AddScoped<IStudentGroupApplicationService, StudentGroupApplicationService>()
                .AddScoped<ICuratorGroupStatisticsService, CuratorGroupStatisticsService>();
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services
                .AddTransient<IValidator<LoginUserRequest>, LoginRequestValidator>()
                .AddTransient<IValidator<RegisterStudentRequest>, RegisterStudentRequestValidator>()
                .AddTransient<IValidator<RegisterCompanyRequest>, RegisterCompanyValidator>()
                .AddTransient<IValidator<RegisterTeacherRequest>, RegisterTeacherValidator>()
                .AddTransient<IValidator<UpdateCompanyRequest>, UpdateCompanyValidator>()
                .AddTransient<IValidator<UpdateStudentProfileRequest>, UpdateStudentProfileValidator>()
                .AddTransient<IValidator<UpdateEmployerProfileRequest>, UpdateEmployerProflieValidator>()
                .AddTransient<IValidator<UpdateTeacherRequest>, UpdateTeacherValidator>()
                
                .AddTransient<IValidator<CreateResumeRequest>, CreateResumeValidator>()
                .AddTransient<IValidator<UpdateResumeRequest>, UpdateResumeValidator>()
                .AddTransient<IValidator<AddWorkExperienceRequest>, AddWorkExperienceValidator>()
                .AddTransient<IValidator<UpdateWorkExperienceRequest>, UpdateWorkExperienceValidator>()
                .AddTransient<IValidator<SearchResumeParameters>, SearchResumeParametersValidator>()
                
                .AddTransient<IValidator<CreateVacancyRequest>, CreateVacancyValidator>()
                .AddTransient<IValidator<UpdateVacancyRequest>, UpdateVacancyValidator>()
                .AddTransient<IValidator<SearchVacancyParameters>, SearchVacancyValidator>()
                
                .AddTransient<IValidator<GetStudentApplicationsParameters>, GetStudentApplicationsValidator>()
                .AddTransient<IValidator<GetEmployerApplicationsParameters>, GetEmployerApplicationsValidator>()
                
                .AddTransient<IValidator<CreateStudentGroupRequest>, CreateStudentGroupValidator>()
                
                .AddTransient<IValidator<CreateStudentGroupApplicationRequest>, CreateStudentGroupApplicationValidator>();
        }

        public static IServiceCollection AddPolicies(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy(Policies.StudentMustHaveGroup, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(Roles.Student);
                    policy.AddRequirements(new StudentMustHaveGroupRequirement());
                });

            services.AddAuthorizationBuilder()
                .AddPolicy(Policies.ApprovedUser, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddRequirements(new ApprovedUserRequirement());
                });

            services
                .AddScoped<IAuthorizationHandler, StudentMustHaveGroupHandler>()
                .AddScoped<IAuthorizationHandler, ApprovedUserHandler>();

            return services;
        }
    }
}
