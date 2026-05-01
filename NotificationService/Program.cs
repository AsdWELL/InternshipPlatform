using NotificationService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEmailSender(builder.Configuration)
    .AddKafka(builder.Configuration);

var app = builder.Build();

app.Run();
