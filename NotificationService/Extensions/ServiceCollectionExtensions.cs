using NotificationService.Dto;
using NotificationService.Interfaces;
using NotificationService.Services;

namespace NotificationService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
        {
            var kafkaSection = configuration.GetRequiredSection("Kafka");
            services.Configure<KafkaSettings>(kafkaSection);

            return services.AddHostedService<NotificationConsumer>();
        }

        public static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSection = configuration.GetRequiredSection("EmailOptions");
            services.Configure<EmailOptions>(emailSection);
            
            return services.AddScoped<IEmailSender, EmailSender>();
        }
    }
}
