using InternshipPlatform.Application.Dtos.JobApplication;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Notifiers;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Services;
using InternshipPlatform.Application.Exceptions.JobApplication;
using InternshipPlatform.Application.Exceptions.Resume;
using InternshipPlatform.Application.Exceptions.Vacancy;
using InternshipPlatform.Application.Values;
using InternshipPlatform.Domain.Entities;
using Moq;

namespace UnitTests
{
    public class JobApplicationServiceTests
    {
        private readonly Mock<IJobApplicationRepository> _applicationRepoMock;
        private readonly Mock<IChatService> _chatServiceMock;
        private readonly Mock<IResumeRepository> _resumeRepoMock;
        private readonly Mock<IVacancyRepository> _vacancyRepoMock;
        private readonly Mock<IEmployerProfileRepository> _employerProfileRepoMock;
        private readonly Mock<IStudentProfileRepository> _studentProfileRepoMock;
        private readonly Mock<IJobApplicationNotifier> _notifierMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly JobApplicationService _service;

        public JobApplicationServiceTests()
        {
            _applicationRepoMock = new Mock<IJobApplicationRepository>();
            _chatServiceMock = new Mock<IChatService>();
            _resumeRepoMock = new Mock<IResumeRepository>();
            _vacancyRepoMock = new Mock<IVacancyRepository>();
            _employerProfileRepoMock = new Mock<IEmployerProfileRepository>();
            _studentProfileRepoMock = new Mock<IStudentProfileRepository>();
            _notifierMock = new Mock<IJobApplicationNotifier>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _service = new JobApplicationService(
                _applicationRepoMock.Object,
                _chatServiceMock.Object,
                _resumeRepoMock.Object,
                _vacancyRepoMock.Object,
                _employerProfileRepoMock.Object,
                _studentProfileRepoMock.Object,
                _notifierMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task UpdateApplicationStatus_InvalidRole_ThrowsInvalidJobApplicationStatusException()
        {
            int studentId = 1;
            int applicationId = 100;
            var application = new JobApplication { Id = applicationId, ApplicationStatusId = (int)JobApplicationStatuses.Pending };

            _applicationRepoMock.Setup(x => x.GetStudentApplicationForUpdate(studentId, applicationId)).ReturnsAsync(application);

            // Студент пытается пригласить себя на интервью
            var request = new UpdateApplicationStatusRequest
            {
                UserId = studentId,
                ApplicationId = applicationId,
                Role = Roles.Student,
                ApplicationStatus = JobApplicationStatuses.InterviewInvited
            };

            await Assert.ThrowsAsync<InvalidJobApplicationStatusException>(() => _service.UpdateApplicationStatus(request));
        }

        [Fact]
        public async Task UpdateApplicationStatus_InvalidTransition_ThrowsInvalidJobApplicationStatusException()
        {
            int employerId = 2;
            int applicationId = 100;
            var application = new JobApplication { Id = applicationId, ApplicationStatusId = (int)JobApplicationStatuses.Pending };

            _applicationRepoMock.Setup(x => x.GetEmployerApplicationForUpdate(employerId, applicationId)).ReturnsAsync(application);

            // Работодатель пытается перескочить трудоустроить студента сразу после формирования отклика
            var request = new UpdateApplicationStatusRequest
            {
                UserId = employerId,
                ApplicationId = applicationId,
                Role = Roles.Employer,
                ApplicationStatus = JobApplicationStatuses.Employed
            };

            await Assert.ThrowsAsync<InvalidJobApplicationStatusException>(() => _service.UpdateApplicationStatus(request));
        }

        [Fact]
        public async Task CreateJobApplicationByStudent_ResumeNotFound_ThrowsResumeNotFoundException()
        {
            var request = new CreateJobApplicationRequest { UserId = 1, ResumeId = 999, VacancyId = 1 };
            _resumeRepoMock.Setup(x => x.IsStudentOwnsResume(request.UserId, request.ResumeId)).ReturnsAsync(false);

            await Assert.ThrowsAsync<ResumeNotFoundException>(() => _service.CreateJobApplicationByStudent(request));
        }

        [Fact]
        public async Task CreateJobApplicationByStudent_VacancyNotFound_ThrowsVacancyNotFoundException()
        {
            var request = new CreateJobApplicationRequest { UserId = 1, ResumeId = 1, VacancyId = 999 };
            _resumeRepoMock.Setup(x => x.IsStudentOwnsResume(request.UserId, request.ResumeId)).ReturnsAsync(true);
            _vacancyRepoMock.Setup(x => x.GetVacancyById(request.VacancyId)).ReturnsAsync((Vacancy?)null);

            await Assert.ThrowsAsync<VacancyNotFoundException>(() => _service.CreateJobApplicationByStudent(request));
        }

        [Fact]
        public async Task CreateJobApplicationByStudent_ActiveApplicationExists_ThrowsActiveApplicationAlreadyExistsException()
        {
            var request = new CreateJobApplicationRequest { UserId = 1, ResumeId = 1, VacancyId = 1 };
            _resumeRepoMock.Setup(x => x.IsStudentOwnsResume(request.UserId, request.ResumeId)).ReturnsAsync(true);
            _vacancyRepoMock.Setup(x => x.GetVacancyById(request.VacancyId)).ReturnsAsync(new Vacancy { Id = 1, IsActive = true, CompanyId = 1 });
            _applicationRepoMock.Setup(x => x.HasStudentActiveApplicationOnVacancy(request.ResumeId, request.VacancyId)).ReturnsAsync(true); // Уже существует

            await Assert.ThrowsAsync<ActiveApplicationAlreadyExistsException>(() => _service.CreateJobApplicationByStudent(request));
        }
    }
}
