using InternshipPlatform.Application.Dtos.Kafka;
using InternshipPlatform.Application.Interfaces.Notifiers;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Services.Notifiers
{
    public class PracticeApplicationNotifier(INotificationProducer notificationProducer) : IPracticeApplicationNotifier
    {
        public Task NotifyEmployerAboutNewApplication(string employerEmail, string practiceOfferTitle)
        {
            return notificationProducer.SendNotificationAsync(new NotificationEvent
            {
                Email = employerEmail,
                Title = "Заявка на практику",
                Message = $"На предложение по практике \"{practiceOfferTitle}\" поступила новая заявка. Посмотрите кто это был."
            });
        }

        public Task NotifyStudentApplicationAccepted(string studentEmail, PracticeOffer practiceOffer)
        {
            return notificationProducer.SendNotificationAsync(new NotificationEvent
            {
                Email = studentEmail,
                Title = "Вы приняты на практику!",
                Message = $"Компания {practiceOffer.Company.Name} приняла вас на прохождение практики \"{practiceOffer.Title}\". " +
                    $"Поздравляем!"
            });
        }

        public Task NotifyStudentApplicationRejected(string studentEmail, PracticeOffer practiceOffer)
        {
            return notificationProducer.SendNotificationAsync(new NotificationEvent
            {
                Email = studentEmail,
                Title = "Отказ по практике",
                Message = $"Компания {practiceOffer.Company.Name} отказала вам в прохождении практики \"{practiceOffer.Title}\". " +
                    $"Не расстраивайтесь и продолжайте поиски!"
            });
        }
    }
}
