using NotificationService.Dto;

namespace NotificationService.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(NotificationEvent notificationEvent);
    }
}
