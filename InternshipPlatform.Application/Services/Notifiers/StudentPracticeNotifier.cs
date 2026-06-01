using InternshipPlatform.Application.Dtos.Kafka;
using InternshipPlatform.Application.Interfaces.Notifiers;
using InternshipPlatform.Application.Interfaces.Services;

namespace InternshipPlatform.Application.Services.Notifiers
{
    public class StudentPracticeNotifier(INotificationProducer notificationProducer) : IStudentPracticeNotifier
    {
        public Task NotifyStudentPracticeAccepted(string studentEmail, string companyName)
        {
            return notificationProducer.SendNotificationAsync(new NotificationEvent
            {
                Email = studentEmail,
                Title = "Производственная практика принята!",
                Message = $"Представитель компании {companyName}, в которой вы проходите практику принял ваше решение. " +
                    "Теперь ожидайет оценнки от преподавателя."
            });
        }

        public Task NotifyStudentPracticeGraded(string studentEmail, int grade)
        {
            return notificationProducer.SendNotificationAsync(new NotificationEvent
            {
                Email = studentEmail,
                Title = "Производственная практика завершена!",
                Message = $"Ваш руководитель практики выставил вам оценку {grade}"
            });
        }

        public Task NotifyStudentPracticeRejected(string studentEmail, string companyName)
        {
            return notificationProducer.SendNotificationAsync(new NotificationEvent
            {
                Email = studentEmail,
                Title = "Производственная практика отправлена на доработку",
                Message = $"Представитель компании {companyName}, отправил ваше решенее по практике на доработку. " +
                    "Зайдите в систему, чтобы узнать свои ошибки."
            });
        }
    }
}
