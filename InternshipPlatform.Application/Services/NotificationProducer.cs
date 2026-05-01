using Confluent.Kafka;
using InternshipPlatform.Application.Dtos.Kafka;
using InternshipPlatform.Application.Json;
using Microsoft.Extensions.Options;

namespace InternshipPlatform.Application.Services
{
    public class NotificationProducer(IOptions<KafkaSettings> kafkaOptions)
    {
        private const string Topic = "PlatformNotifications";

        public async Task SendNotificationAsync(NotificationEvent notificationEvent)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = kafkaOptions.Value.BootstrapServers,
                AllowAutoCreateTopics = true,
                Acks = Acks.All
            };

            using (var producer = new ProducerBuilder<Null, NotificationEvent>(config)
                .SetValueSerializer(new JsonSerializer<NotificationEvent>())
                .Build())
            {
                await producer.ProduceAsync(topic: Topic,
                    new Message<Null, NotificationEvent>
                    {
                        Value = notificationEvent
                    });

                producer.Flush();
            }
        }
    }
}
