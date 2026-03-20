using InternshipPlatform.Application.Dtos.Auth;
using InternshipPlatform.Application.Interfaces.Services.Auth;
using InternshipPlatform.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace InternshipPlatform.Api.Extensions
{
    public static class JwtExtension
    {        
        public static IServiceCollection AddJWTAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenService, TokenService>()
                    .AddScoped<IPasswordHasher, PasswordHasher>();
            
            var tokenSection = configuration.GetRequiredSection(nameof(TokenOptions));

            services.Configure<TokenOptions>(tokenSection);

            var tokenOptions = tokenSection.Get<TokenOptions>() ??
                throw new InvalidOperationException("Секция 'TokenOptions' отстутствует или имеет неверный формат");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.ClaimsIssuer = tokenOptions.Issuer;
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Key))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies[tokenOptions.CookieTitle];

                            return Task.CompletedTask;
                        },

                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception is SecurityTokenExpiredException)
                                context.Response.Headers.Append("Token-Expired", "true");

                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}
