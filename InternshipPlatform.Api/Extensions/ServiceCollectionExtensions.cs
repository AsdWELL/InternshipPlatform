using FluentValidation;
using InternshipPlatform.Application.Dtos.Company;
using InternshipPlatform.Application.Dtos.EmployerProflie;
using InternshipPlatform.Application.Dtos.StudentProfile;
using InternshipPlatform.Application.Dtos.User;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Interfaces.Services.Auth;
using InternshipPlatform.Application.Services;
using InternshipPlatform.Application.Validators.Auth;
using InternshipPlatform.Application.Validators.EmployerProfile;
using InternshipPlatform.Application.Validators.StudentProfile;
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

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IImageService, ImageService>()
                .AddScoped<IAuthService, AuthService>()
                .AddScoped<IStudentProfileService, StudentProfileService>()
                .AddScoped<ICompanyService, CompanyService>()
                .AddScoped<IEmployerProflieService, EmployerProflieService>();
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services
                .AddScoped<IValidator<LoginUserRequest>, LoginRequestValidator>()
                .AddScoped<IValidator<RegisterStudentRequest>, RegisterStudentRequestValidator>()
                .AddScoped<IValidator<RegisterCompanyRequest>, RegisterCompanyValidator>()
                .AddScoped<IValidator<UpdateCompanyRequest>, UpdateCompanyValidator>()
                .AddScoped<IValidator<UpdateStudentProfileRequest>, UpdateStudentProfileValidator>()
                .AddScoped<IValidator<UpdateEmployerProflieRequest>, UpdateEmployerProflieValidator>();
        }
    }
}
