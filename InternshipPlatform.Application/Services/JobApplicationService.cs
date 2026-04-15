using InternshipPlatform.Application.Dtos.JobApplication;
using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Exceptions.JobApplication;
using InternshipPlatform.Application.Exceptions.Resume;
using InternshipPlatform.Application.Exceptions.Vacancy;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;
using InternshipPlatform.Application.Values;

namespace InternshipPlatform.Application.Services
{
    public class JobApplicationService(
        IJobApplicationRepository applicationRepository,
        IResumeRepository resumeRepository,
        IVacancyRepository vacancyRepository,
        IUnitOfWork unitOfWork) : IJobApplicationService
    {
        public async Task<int> CreateJobApplicationByStudent(CreateJobApplicaionRequest request)
        {
            if (!await resumeRepository.IsStudentOwnsResume(request.UserId, request.ResumeId))
                throw new ResumeNotFoundException();

            if (!await vacancyRepository.IsVacancyExistsAndActive(request.VacancyId))
                throw new VacancyNotFoundException();
            
            var application = request.ToDomain(Roles.Student);
            await applicationRepository.AddJobApplication(application);

            await unitOfWork.SaveChangesAsync();

            return application.Id;
        }

        public async Task<int> CreateJobApplicationByEmployer(CreateJobApplicaionRequest request)
        {
            if (!await vacancyRepository.IsEmployerOwnsVacancy(request.UserId, request.VacancyId))
                throw new VacancyNotFoundException();

            if (!await resumeRepository.IsResumeExistsAndActive(request.ResumeId))
                throw new ResumeNotFoundException();

            var application = request.ToDomain(Roles.Employer);
            await applicationRepository.AddJobApplication(application);

            await unitOfWork.SaveChangesAsync();

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

            if (!IsValidStatusTransition(request.Role, currentStatus, request.ApplicationStatus))
                throw new InvalidJobApplicationStatusException();

            application.ApplicationStatusId = (int)request.ApplicationStatus;
            application.LastStatusDate = DateOnly.FromDateTime(DateTime.UtcNow);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
