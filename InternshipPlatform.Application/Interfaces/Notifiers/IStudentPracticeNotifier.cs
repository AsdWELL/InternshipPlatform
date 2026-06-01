namespace InternshipPlatform.Application.Interfaces.Notifiers
{
    public interface IStudentPracticeNotifier
    {
        Task NotifyStudentPracticeAccepted(string studentEmail, string companyName);

        Task NotifyStudentPracticeRejected(string studentEmail, string companyName);

        Task NotifyStudentPracticeGraded(string studentEmail, int grade);
    }
}
