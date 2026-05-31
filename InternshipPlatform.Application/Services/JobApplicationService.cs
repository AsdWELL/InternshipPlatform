using InternshipPlatform.Application.Dtos.Chat;
using InternshipPlatform.Application.Dtos.JobApplication;
using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Exceptions.JobApplication;
using InternshipPlatform.Application.Exceptions.Resume;
using InternshipPlatform.Application.Exceptions.Vacancy;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Notifiers;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;
using InternshipPlatform.Application.Values;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Services
{
    public class JobApplicationService(
        IJobApplicationRepository applicationRepository,
        IChatService chatService,
        IResumeRepository resumeRepository,
        IVacancyRepository vacancyRepository,
        IEmployerProfileRepository employerProfileRepository,
        IStudentProfileRepository studentProfileRepository,
        IJobApplicationNotifier jobApplicationNotifier,
        IUnitOfWork unitOfWork) : IJobApplicationService
    {
        public async Task<int> CreateJobApplicationByStudent(CreateJobApplicationRequest request)
        {
            if (!await resumeRepository.IsStudentOwnsResume(request.UserId, request.ResumeId))
                throw new ResumeNotFoundException();

            var vacancy = await vacancyRepository.GetVacancyById(request.VacancyId);

            if (vacancy is null || !vacancy.IsActive)
                throw new VacancyNotFoundException();

            if (await applicationRepository.HasStudentActiveApplicationOnVacancy(request.ResumeId, request.VacancyId))
                throw new ActiveApplicationAlreadyExistsException();

            var chatId = await chatService.GetOrCreateStudentChat(new StartStudentChatRequest
            {
                StudentId = request.UserId,
                VacancyId = request.VacancyId,
                Message = request.WelcomeMessage
            });
            
            var application = request.ToDomain(Roles.Student, chatId);
            await applicationRepository.AddJobApplication(application);

            await unitOfWork.SaveChangesAsync();

            var employerEmail = await employerProfileRepository.GetEmployerEmailByCompanyId(vacancy.CompanyId);

            if (!string.IsNullOrEmpty(employerEmail))
                await jobApplicationNotifier.NotifyEmployerAboutStatusChangedAsync(employerEmail, vacancy.Title, JobApplicationStatuses.Pending);

            return application.Id;
        }

        public async Task<int> CreateJobApplicationByEmployer(CreateJobApplicationRequest request)
        {
            if (!await vacancyRepository.IsEmployerOwnsVacancy(request.UserId, request.VacancyId))
                throw new VacancyNotFoundException();

            var resume = await resumeRepository.GetResumeById(request.ResumeId);
            
            if (resume is null || !resume.IsActive) 
                throw new ResumeNotFoundException();

            if (await applicationRepository.HasStudentActiveApplicationOnVacancy(request.ResumeId, request.VacancyId))
                throw new ActiveApplicationAlreadyExistsException();

            var chatId = await chatService.GetOrCreateEmployerChat(new StartEmployerChatRequest
            {
                EmployerId = request.UserId,
                StudentId = resume.StudentId,
                VacancyId = request.VacancyId,
                Message = request.WelcomeMessage
            });

            var application = request.ToDomain(Roles.Employer, chatId);
            await applicationRepository.AddJobApplication(application);

            await unitOfWork.SaveChangesAsync();

            var vacancy = await vacancyRepository.GetVacancyById(request.VacancyId)
                ?? throw new VacancyNotFoundException();

            var studentEmail = await studentProfileRepository.GetStudentEmailById(resume.StudentId);

            if (!string.IsNullOrEmpty(studentEmail))
                await jobApplicationNotifier.NotifyStudentAboutStatusChangedAsync(studentEmail, vacancy, JobApplicationStatuses.InterviewInvited);

            return application.Id;
        }

        public async Task<PagedResponse<StudentApplicationResponse>> GetStudentApplications(GetStudentApplicationsParameters parameters)
        {
            if (parameters.ResumeId.HasValue
                && !await resumeRepository.IsStudentOwnsResume(parameters.StudentId, parameters.ResumeId.Value))
                    throw new ResumeNotFoundException();

            var applications = await applicationRepository.GetStudentApplications(parameters);

            return applications.ToPagedResponse(parameters, a => a.ToStudentApplication());
        }

        public async Task<PagedResponse<EmployerApplicationResponse>> GetEmployerApplications(GetEmployerApplicationsParameters parameters)
        {
            if (parameters.VacancyId.HasValue
                && !await vacancyRepository.IsEmployerOwnsVacancy(parameters.EmployerId, parameters.VacancyId.Value))
                throw new VacancyNotFoundException();

            var applications = await applicationRepository.GetEmployerApplications(parameters);

            return applications.ToPagedResponse(parameters, a => a.ToEmployerApplication());
        }

        private bool IsValidStatusTransition(string role, JobApplicationStatuses currentStatus, JobApplicationStatuses newStatus)
        {
            if (currentStatus == newStatus)
                return false;

            return (currentStatus, newStatus, role) switch
            {
                (JobApplicationStatuses.Pending, JobApplicationStatuses.InterviewInvited, Roles.Employer) => true,
                (JobApplicationStatuses.Pending, JobApplicationStatuses.Rejected, Roles.Employer) => true,
                (JobApplicationStatuses.Pending, JobApplicationStatuses.Withdrawn, Roles.Student) => true,

                (JobApplicationStatuses.InterviewInvited, JobApplicationStatuses.OfferReceived, Roles.Employer) => true,
                (JobApplicationStatuses.InterviewInvited, JobApplicationStatuses.Rejected, Roles.Employer) => true,
                (JobApplicationStatuses.InterviewInvited, JobApplicationStatuses.Withdrawn, Roles.Student) => true,

                (JobApplicationStatuses.OfferReceived, JobApplicationStatuses.Accepted, Roles.Student) => true,
                (JobApplicationStatuses.OfferReceived, JobApplicationStatuses.Rejected, Roles.Employer) => true,
                (JobApplicationStatuses.OfferReceived, JobApplicationStatuses.Withdrawn, Roles.Student) => true,

                (JobApplicationStatuses.Accepted, JobApplicationStatuses.Employed, Roles.Employer) => true,
                (JobApplicationStatuses.Accepted, JobApplicationStatuses.Withdrawn, Roles.Student) => true,

                _ => false
            };
        }

        public async Task UpdateApplicationStatus(UpdateApplicationStatusRequest request)
        {
            var application = request.Role switch
            {
                Roles.Student => await applicationRepository.GetStudentApplicationForUpdate(
                    request.UserId,
                    request.ApplicationId),
                
                Roles.Employer => await applicationRepository.GetEmployerApplicationForUpdate(
                    request.UserId,
                    request.ApplicationId),
                
                _ => null
            } ?? throw new JobApplicationNotFoundException();

            var currentStatus = (JobApplicationStatuses)application.ApplicationStatusId;

            if (!IsValidStatusTransition(request.Role!, currentStatus, request.ApplicationStatus))
                throw new InvalidJobApplicationStatusException();

            application.ApplicationStatusId = (int)request.ApplicationStatus;
            application.LastStatusDate = DateOnly.FromDateTime(DateTime.UtcNow);

            if (request.ApplicationStatus == JobApplicationStatuses.Rejected ||
                request.ApplicationStatus == JobApplicationStatuses.Withdrawn)
                await chatService.CloseChatByApplicationId(request.UserId, request.Role!, request.ApplicationId);

            await unitOfWork.SaveChangesAsync();

            var vacancy = await vacancyRepository.GetVacancyById(application.VacancyId)
                ?? throw new VacancyNotFoundException();

            if (request.Role == Roles.Employer)
            {
                var studentEmail = await resumeRepository.GetStudentEmailByResumeId(application.ResumeId);
                if (!string.IsNullOrEmpty(studentEmail))
                    await jobApplicationNotifier.NotifyStudentAboutStatusChangedAsync(studentEmail, vacancy, request.ApplicationStatus);
            }
            else if (request.Role == Roles.Student)
            {
                var employerEmail = await employerProfileRepository.GetEmployerEmailByCompanyId(vacancy.CompanyId);
                if (!string.IsNullOrEmpty(employerEmail))
                    await jobApplicationNotifier.NotifyEmployerAboutStatusChangedAsync(employerEmail, vacancy.Title, request.ApplicationStatus);
            }
        }
    }
}
