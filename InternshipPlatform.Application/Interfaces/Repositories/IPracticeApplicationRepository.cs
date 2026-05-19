using InternshipPlatform.Application.Dtos.PracticeApplication;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IPracticeApplicationRepository
    {
        Task AddPracticeApplication(PracticeApplication application);

        Task<PracticeApplicationResult?> GetCurrentStudentPracticeApplication(int studentId);

        Task<bool> HasActivePracticeApplication(int studentId);

        Task<PracticeApplication?> GetStudentPracticeApplicationForUpdate(int studentId, int applicationId);

        Task<List<PracticeApplicationResult>> GetEmployerPracticeApplications(int employerId);

        Task<PracticeApplicationResult?> GetEmployerPracticeApplicationDetails(int employerId, int applicationId);

        Task<PracticeApplicationResult?> GetEmployerPracticeApplicationForUpdate(int employerId, int applicationId);

        Task<bool> HasPracticeOfferAvailablePlaces(int practiceOfferId);

        Task<bool> IsStudentAlreadyEnrolledInPracticePeriod(int studentId,int practicePeriodId);

        Task<List<PracticeApplication>> GetPracticeOfferApplicationsForUpdate(int practiceOfferId);

        Task DeletePracticeApplication(int applicationId);

        Task DeletePracticeApplications(List<PracticeApplication> applications);
    }
}