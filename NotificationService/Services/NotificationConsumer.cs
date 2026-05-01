using Confluent.Kafka;
using Microsoft.Extensions.Options;
using NotificationService.Dto;
using NotificationService.Interfaces;
using NotificationService.Json;

namespace NotificationService.Services
{
    public class NotificationConsumer(
        IOptions<KafkaSettings> kafkaOptions,
        IServiceScopeFactory scopeFactory) : BackgroundService
    {
        private const string Topic = "PlatformNotifications";

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = "InternshipPlatform",
                BootstrapServers = kafkaOptions.Value.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };

            Task.Run(async () =>
            {
                using (var consumer = new ConsumerBuilder<Ignore, NotificationEvent>(config)
                .SetValueDeserializer(new JsonDeserializer<NotificationEvent>())
                .Build())
                {
                    consumer.Subscribe(Topic);

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var consumerResult = consumer.Consume(TimeSpan.FromSeconds(5));

                        if (consumerResult == null)
                            continue;

                        using var scope = scopeFactory.CreateScope();

                        var emailSender = scope.ServiceProvider
                            .GetRequiredService<IEmailSender>();

                        var emailMessage = consumerResult.Message.Value;

                        await emailSender.SendEmailAsync(emailMessage);

                        consumer.Commit(consumerResult);
                    }
                }
            }, stoppingToken);

            return Task.CompletedTask;
        }
    }
}
