using InternshipPlatform.Api.Extensions;
using InternshipPlatform.Api.Filters;
using InternshipPlatform.Infrastructure.Extensions;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwagger();

builder.Services.AddDatabase(builder.Configuration)
                .AddRepositories()
                .AddServices()
                .AddValidators();

builder.Services.AddJWTAuth(builder.Configuration);

builder.Services.AddFluentValidationAutoValidation();

builder.Services
    .AddControllers(options => options.Filters.Add<ExceptionFilter>());

var app = builder.Build();

app.UseCookiePolicy(new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.SameAsRequest
});

app.UseSwagger()
   .UseSwaggerUI(options =>
   {
       options.DisplayRequestDuration();
       options.SwaggerEndpoint("/swagger/InternshipPlatform/swagger.json", "InternshipPlatform");
       options.RoutePrefix = string.Empty;
   });


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
