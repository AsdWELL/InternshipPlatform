using InternshipPlatform.Api.Extensions;
using InternshipPlatform.Api.Filters;
using InternshipPlatform.Api.Hubs;
using InternshipPlatform.Infrastructure.Extensions;
using InternshipPlatform.Infrastructure.Migration;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "wwwroot"
});

builder.Services.AddSwagger();

builder.Services.AddDatabase(builder.Configuration)
                .AddRepositories()
                .AddServices()
                .AddValidators()
                .AddKafka(builder.Configuration);

builder.Services
    .AddJWTAuth(builder.Configuration)
    .AddPolicies();

builder.Services
    .AddFluentValidationAutoValidation()
    .AddControllers(options => options.Filters.Add<ExceptionFilter>());

builder.Services.AddSignalR();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var seeder = services.GetRequiredService<DbSeeder>();

    seeder.Seed();

    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Ѕаза данных успешно заполнена тестовыми данными.");
}

app.UseCors(cors =>
{
    cors.WithOrigins("http://localhost:5173")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

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


app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");

app.Run();
