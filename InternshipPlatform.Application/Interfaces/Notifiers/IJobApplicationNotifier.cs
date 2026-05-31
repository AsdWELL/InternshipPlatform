using InternshipPlatform.Application.Values;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Notifiers
{
    public interface IJobApplicationNotifier
    {
        Task NotifyEmployerAboutStatusChangedAsync(string employerEmail, string vacancyTitle, JobApplicationStatuses status);
        
        Task NotifyStudentAboutStatusChangedAsync(string studentEmail, Vacancy vacancy, JobApplicationStatuses status);
    }
}
