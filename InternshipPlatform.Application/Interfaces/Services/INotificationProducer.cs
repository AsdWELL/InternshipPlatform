using InternshipPlatform.Application.Dtos.Kafka;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface INotificationProducer
    {
        Task SendNotificationAsync(NotificationEvent notificationEvent);
    }
}
