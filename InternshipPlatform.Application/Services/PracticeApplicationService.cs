using InternshipPlatform.Application.Dtos.PracticeApplication;
using InternshipPlatform.Application.Exceptions.PracticeApplication;
using InternshipPlatform.Application.Exceptions.PracticeOffer;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Services
{
    public class PracticeApplicationService(
        IPracticeApplicationRepository practiceApplicationRepository,
        IStudentPracticeRepository studentPracticeRepository,
        IPracticePeriodRepository practicePeriodRepository,
        IPracticeOfferRepository practiceOfferRepository,
        IUnitOfWork unitOfWork) : IPracticeApplicationService
    {
        private static bool IsApplicationPeriodClosed(PracticePeriod practicePeriod)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            return practicePeriod.StartDate <= today;
        }

        public async Task<int> CreatePracticeApplication(CreatePracticeApplicationRequest request)
        {
            if (await practiceApplicationRepository.HasActivePracticeApplication(request.StudentId))
                throw new ActivePracticeApplicationAlreadyExistsException();

            var practicePeriod = await practicePeriodRepository.GetCurrentStudentPracticePeriod(request.StudentId)
                ?? throw new CurrentSemesterPracticePeriodNotFoundException();

            if (IsApplicationPeriodClosed(practicePeriod))
                throw new PracticeApplicationPeriodAlreadyStartedException();

            if (!await practiceOfferRepository.IsPracticeOfferExistsAndActive(request.PracticeOfferId))
                throw new PracticeOfferNotFoundException();

            if (!await practiceApplicationRepository.HasPracticeOfferAvailablePlaces(request.PracticeOfferId))
                throw new PracticeOfferHasNoAvailablePlacesException();

            if (await practiceApplicationRepository.IsStudentAlreadyEnrolledInPracticePeriod(request.StudentId, practicePeriod.Id))
                throw new StudentAlreadyEnrolledInPracticeException();

            var application = request.ToDomain(practicePeriod);

            await practiceApplicationRepository.AddPracticeApplication(application);
            await unitOfWork.SaveChangesAsync();

            return application.Id;
        }

        public async Task<StudentPracticeApplicationItem?> GetCurrentStudentPracticeApplication(int studentId)
        {
            var application = await practiceApplicationRepository.GetCurrentStudentPracticeApplication(studentId);

            return application?.ToStudentItem();
        }

        public async Task CancelPracticeApplication(int studentId, int applicationId)
        {
            var application = await practiceApplicationRepository
                .GetStudentPracticeApplicationForUpdate(studentId, applicationId) 
                ?? throw new PracticeApplicationNotFoundException();

            await practiceApplicationRepository.DeletePracticeApplication(application.Id);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<EmployerPracticeApplicationItem>> GetEmployerPracticeApplications(int employerId)
        {
            var applications = await practiceApplicationRepository.GetEmployerPracticeApplications(employerId);

            return applications
                .Select(a => a.ToEmployerItem())
                .OrderByDescending(a => a.CreatedAt)
                .ToList();
        }

        public async Task<EmployerPracticeApplicationDetails> GetEmployerPracticeApplicationDetails(int employerId, int applicationId)
        {
            var application = await practiceApplicationRepository
                .GetEmployerPracticeApplicationDetails(employerId, applicationId) 
                ?? throw new PracticeApplicationNotFoundException();

            return application.ToEmployerDetails();
        }

        public async Task AcceptPracticeApplication(int employerId, int applicationId)
        {
            var result = await practiceApplicationRepository
                .GetEmployerPracticeApplicationForUpdate(employerId, applicationId) 
                ?? throw new PracticeApplicationNotFoundException();

            if (result.AvailablePlaces <= 0)
                throw new PracticeOfferHasNoAvailablePlacesException();

            var application = result.PracticeApplication;
            
            if (await practiceApplicationRepository
                .IsStudentAlreadyEnrolledInPracticePeriod(application.StudentId, application.PracticePeriodId))
                throw new StudentAlreadyEnrolledInPracticeException();

            await studentPracticeRepository.AddStudentPractice(new StudentPractice
            {
                StudentId = application.StudentId,
                PracticePeriodId = application.PracticePeriodId,
                PracticeOfferId = application.PracticeOfferId
            });

            if (result.AvailablePlaces == 1)
            {
                var otherApplications = await practiceApplicationRepository.GetPracticeOfferApplicationsForUpdate(
                    application.PracticeOfferId);

                await practiceApplicationRepository.DeletePracticeApplications(otherApplications);
            }
            else
                await practiceApplicationRepository.DeletePracticeApplication(application.Id);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task RejectPracticeApplication(int employerId, int applicationId)
        {
            var result = await practiceApplicationRepository
                .GetEmployerPracticeApplicationForUpdate(employerId, applicationId) 
                ?? throw new PracticeApplicationNotFoundException();

            await practiceApplicationRepository.DeletePracticeApplication(result.PracticeApplication.Id);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
