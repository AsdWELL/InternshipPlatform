namespace InternshipPlatform.Application.Interfaces.Notifiers
{
    public interface IStudentGroupApplicationNotifier
    {
        Task NotifyCuratorAboutNewApplication(string curatorEmail, string studentFullName, string groupName);
        
        Task NotifyStudentApplicationAccepted(string studentEmail, string groupName);
        
        Task NotifyStudentApplicationRejected(string studentEmail, string groupName);
    }
}
