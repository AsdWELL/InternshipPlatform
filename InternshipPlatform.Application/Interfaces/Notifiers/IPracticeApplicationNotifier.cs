using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Notifiers
{
    public interface IPracticeApplicationNotifier
    {
        Task NotifyEmployerAboutNewApplication(string employerEmail, string practiceOfferTitle);

        Task NotifyStudentApplicationAccepted(string studentEmail, PracticeOffer practiceOffer);

        Task NotifyStudentApplicationRejected(string studentEmail, PracticeOffer practiceOffer);
    }
}
