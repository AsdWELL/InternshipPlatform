using InternshipPlatform.Application.Dtos.Kafka;
using InternshipPlatform.Application.Interfaces.Notifiers;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Services.Notifiers
{
    public class JobApplicationNotifier(INotificationProducer notificationProducer) : IJobApplicationNotifier
    {
        public async Task NotifyEmployerAboutStatusChangedAsync(string employerEmail, string vacancyTitle, JobApplicationStatuses status)
        {
            var notification = status switch
            {
                JobApplicationStatuses.Pending => new NotificationEvent
                {
                    Email = employerEmail,
                    Title = "Новый отклик на вакансию!",
                    Message = $"На вашу вакансию \"{vacancyTitle}\" поступил новый отклик. Посмотрите кто это был."
                },
                JobApplicationStatuses.Accepted => new NotificationEvent
                {
                    Email = employerEmail,
                    Title = "Оффер принят!",
                    Message = $"Кандидат принял ваш оффер на вакансию \"{vacancyTitle}\"."
                },
                JobApplicationStatuses.Withdrawn => new NotificationEvent
                {
                    Email = employerEmail,
                    Title = "Отклик отозван",
                    Message = $"Кандидат отозвал свой отклик на вакансию \"{vacancyTitle}\"."
                },
                _ => null
            };

            if (notification != null)
                await notificationProducer.SendNotificationAsync(notification);
        }

        public async Task NotifyStudentAboutStatusChangedAsync(string studentEmail, Vacancy vacancy, JobApplicationStatuses status)
        {
            var notification = status switch
            {
                JobApplicationStatuses.InterviewInvited => new NotificationEvent
                {
                    Email = studentEmail,
                    Title = "Приглашение на собеседование!",
                    Message = $"Вас пригласили на собеседование в компанию {vacancy.Company.Name} по вакансии \"{vacancy.Title}\". Работодатель скоро свяжется с вами, или вы можете написать ему в чате."
                },
                JobApplicationStatuses.Rejected => new NotificationEvent
                {
                    Email = studentEmail,
                    Title = "Отказ по отклику",
                    Message = $"К сожалению, ваш отклик на вакансию \"{vacancy.Title}\" был отклонен. Не расстраивайтесь и продолжайте поиски!"
                },
                JobApplicationStatuses.OfferReceived => new NotificationEvent
                {
                    Email = studentEmail,
                    Title = "Вам сделали оффер!",
                    Message = $"Поздравляем! Вам сделали оффер в компанию {vacancy.Company.Name} по вакансии \"{vacancy.Title}\"."
                },
                JobApplicationStatuses.Employed => new NotificationEvent
                {
                    Email = studentEmail,
                    Title = "Вы успешно трудоустроены!",
                    Message = $"Поздравляем! Вы официально приняты на работу в компанию {vacancy.Company.Name} по вакансии \"{vacancy.Title}\"."
                },
                _ => null
            };

            if (notification != null)
                await notificationProducer.SendNotificationAsync(notification);
        }
    }
}
