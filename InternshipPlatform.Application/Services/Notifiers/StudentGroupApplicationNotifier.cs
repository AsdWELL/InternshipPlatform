using InternshipPlatform.Application.Dtos.Kafka;
using InternshipPlatform.Application.Interfaces.Notifiers;
using InternshipPlatform.Application.Interfaces.Services;

namespace InternshipPlatform.Application.Services.Notifiers
{
    public class StudentGroupApplicationNotifier(INotificationProducer notificationProducer) : IStudentGroupApplicationNotifier
    {
        public Task NotifyCuratorAboutNewApplication(string curatorEmail, string studentFullName, string groupName)
        {
            return notificationProducer.SendNotificationAsync(new NotificationEvent
            {
                Email = curatorEmail,
                Title = "Новая заявка в учебную группу",
                Message = $"Студент {studentFullName} подал заявку на вступление в вашу учебную группу \"{groupName}\". " +
                    $"Зайдите в систему, чтобы рассмотреть её."
            });
        }

        public Task NotifyStudentApplicationAccepted(string studentEmail, string groupName)
        {
            return notificationProducer.SendNotificationAsync(new NotificationEvent
            {
                Email = studentEmail,
                Title = "Заявка в учебную группу одобрена!",
                Message = $"Куратор принял вашу заявку на вступление в учебную группу \"{groupName}\". " +
                    $"Теперь вам открыт доступ ко всем возможностям платформы!"
            });
        }

        public Task NotifyStudentApplicationRejected(string studentEmail, string groupName)
        {
            return notificationProducer.SendNotificationAsync(new NotificationEvent
            {
                Email = studentEmail,
                Title = "Заявка в учебную группу отклонена",
                Message = $"Куратор отклонил вашу заявку на вступление в учебную группу \"{groupName}\"."
            });
        }
    }
}
