using InternshipPlatform.Application.Dtos.Chat;
using InternshipPlatform.Application.Dtos.JobApplication;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Notifiers;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Services;
using InternshipPlatform.Application.Values;
using InternshipPlatform.Domain.Entities;
using Moq;

namespace UnitTests
{
    public class JobApplicationFullLifecycleTests
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

        public JobApplicationFullLifecycleTests()
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
        public async Task StudentEmployed_Scenario()
        {
            int studentId = 1;
            int employerId = 2;
            int resumeId = 1;
            int vacancyId = 1;
            int expectedApplicationId = 100;
            int chatId = 50;

            JobApplication? storedApplication = null;

            _resumeRepoMock.Setup(x => x.IsStudentOwnsResume(studentId, resumeId))
                .ReturnsAsync(true);

            _vacancyRepoMock.Setup(x => x.GetVacancyById(vacancyId))
                .ReturnsAsync(new Vacancy { Id = vacancyId, IsActive = true, CompanyId = 1 });

            _applicationRepoMock.Setup(x => x.HasStudentActiveApplicationOnVacancy(resumeId, vacancyId))
                .ReturnsAsync(false);

            _chatServiceMock.Setup(x => x.GetOrCreateStudentChat(It.IsAny<StartStudentChatRequest>()))
                .ReturnsAsync(chatId);

            _applicationRepoMock.Setup(x => x.AddJobApplication(It.IsAny<JobApplication>()))
                .Callback<JobApplication>(app =>
                {
                    app.Id = expectedApplicationId;
                    storedApplication = app;
                })
                .Returns(Task.CompletedTask);

            _applicationRepoMock.Setup(x => x.GetEmployerApplicationForUpdate(employerId, expectedApplicationId))
                .ReturnsAsync(() => storedApplication);

            _applicationRepoMock.Setup(x => x.GetStudentApplicationForUpdate(studentId, expectedApplicationId))
                .ReturnsAsync(() => storedApplication);


            // Шаг 1: Студент создает отклик
            var createRequest = new CreateJobApplicationRequest
            {
                UserId = studentId,
                ResumeId = resumeId,
                VacancyId = vacancyId,
                WelcomeMessage = "Привет!"
            };

            int createdAppId = await _service.CreateJobApplicationByStudent(createRequest);
            Assert.Equal(expectedApplicationId, createdAppId);
            Assert.NotNull(storedApplication);
            Assert.Equal((int)JobApplicationStatuses.Pending, storedApplication.ApplicationStatusId);

            // Шаг 2: Работодатель приглашает на собеседование
            await _service.UpdateApplicationStatus(new UpdateApplicationStatusRequest
            {
                UserId = employerId,
                ApplicationId = createdAppId,
                Role = Roles.Employer,
                ApplicationStatus = JobApplicationStatuses.InterviewInvited
            });
            Assert.Equal((int)JobApplicationStatuses.InterviewInvited, storedApplication.ApplicationStatusId);

            // Шаг 3: Работодатель делает оффер
            await _service.UpdateApplicationStatus(new UpdateApplicationStatusRequest
            {
                UserId = employerId,
                ApplicationId = createdAppId,
                Role = Roles.Employer,
                ApplicationStatus = JobApplicationStatuses.OfferReceived
            });
            Assert.Equal((int)JobApplicationStatuses.OfferReceived, storedApplication.ApplicationStatusId);

            // Шаг 4: Студент принимает приглашение
            await _service.UpdateApplicationStatus(new UpdateApplicationStatusRequest
            {
                UserId = studentId,
                ApplicationId = createdAppId,
                Role = Roles.Student,
                ApplicationStatus = JobApplicationStatuses.Accepted
            });
            Assert.Equal((int)JobApplicationStatuses.Accepted, storedApplication.ApplicationStatusId);

            // Шаг 5: Работодатель подтверждает трудоустройство
            await _service.UpdateApplicationStatus(new UpdateApplicationStatusRequest
            {
                UserId = employerId,
                ApplicationId = createdAppId,
                Role = Roles.Employer,
                ApplicationStatus = JobApplicationStatuses.Employed
            });

            // Финальный шаг: проверка статуса
            Assert.Equal((int)JobApplicationStatuses.Employed, storedApplication.ApplicationStatusId);
            _chatServiceMock.Verify(x => x.CloseChatByApplicationId(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task ImmediateRejection_Scenario()
        {
            int studentId = 1;
            int employerId = 2;
            int resumeId = 1;
            int vacancyId = 1;
            int expectedApplicationId = 101;
            int chatId = 51;

            JobApplication? storedApplication = null;

            _resumeRepoMock.Setup(x => x.IsStudentOwnsResume(studentId, resumeId)).ReturnsAsync(true);
            _vacancyRepoMock.Setup(x => x.GetVacancyById(vacancyId)).ReturnsAsync(new Vacancy { Id = vacancyId, IsActive = true, CompanyId = 1 });
            _applicationRepoMock.Setup(x => x.HasStudentActiveApplicationOnVacancy(resumeId, vacancyId)).ReturnsAsync(false);
            _chatServiceMock.Setup(x => x.GetOrCreateStudentChat(It.IsAny<StartStudentChatRequest>())).ReturnsAsync(chatId);

            _applicationRepoMock.Setup(x => x.AddJobApplication(It.IsAny<JobApplication>())).Callback<JobApplication>(app =>
            {
                app.Id = expectedApplicationId;
                storedApplication = app;
            }).Returns(Task.CompletedTask);

            _applicationRepoMock.Setup(x => x.GetEmployerApplicationForUpdate(employerId, expectedApplicationId)).ReturnsAsync(() => storedApplication);
            _applicationRepoMock.Setup(x => x.GetStudentApplicationForUpdate(studentId, expectedApplicationId)).ReturnsAsync(() => storedApplication);

            // Шаг 1: Создание отклика
            var createRequest = new CreateJobApplicationRequest { UserId = studentId, ResumeId = resumeId, VacancyId = vacancyId };
            int createdAppId = await _service.CreateJobApplicationByStudent(createRequest);

            // Шаг 2: Работодатель сразу отказывает
            await _service.UpdateApplicationStatus(new UpdateApplicationStatusRequest
            {
                UserId = employerId,
                ApplicationId = createdAppId,
                Role = Roles.Employer,
                ApplicationStatus = JobApplicationStatuses.Rejected
            });

            // Финальный шаг: проверка статуса
            Assert.Equal((int)JobApplicationStatuses.Rejected, storedApplication!.ApplicationStatusId);
            _chatServiceMock.Verify(x => x.CloseChatByApplicationId(employerId, Roles.Employer, createdAppId), Times.Once);
        }

        [Fact]
        public async Task RejectionAfterInterview_Scenario()
        {
            int studentId = 1;
            int employerId = 2;
            int resumeId = 1;
            int vacancyId = 1;
            int expectedApplicationId = 102;
            int chatId = 52;

            JobApplication? storedApplication = null;

            _resumeRepoMock.Setup(x => x.IsStudentOwnsResume(studentId, resumeId)).ReturnsAsync(true);
            _vacancyRepoMock.Setup(x => x.GetVacancyById(vacancyId)).ReturnsAsync(new Vacancy { Id = vacancyId, IsActive = true, CompanyId = 1 });
            _applicationRepoMock.Setup(x => x.HasStudentActiveApplicationOnVacancy(resumeId, vacancyId)).ReturnsAsync(false);
            _chatServiceMock.Setup(x => x.GetOrCreateStudentChat(It.IsAny<StartStudentChatRequest>())).ReturnsAsync(chatId);

            _applicationRepoMock.Setup(x => x.AddJobApplication(It.IsAny<JobApplication>())).Callback<JobApplication>(app =>
            {
                app.Id = expectedApplicationId;
                storedApplication = app;
            }).Returns(Task.CompletedTask);

            _applicationRepoMock.Setup(x => x.GetEmployerApplicationForUpdate(employerId, expectedApplicationId)).ReturnsAsync(() => storedApplication);
            _applicationRepoMock.Setup(x => x.GetStudentApplicationForUpdate(studentId, expectedApplicationId)).ReturnsAsync(() => storedApplication);

            // Шаг 1: Создание отклика
            int createdAppId = await _service.CreateJobApplicationByStudent(new CreateJobApplicationRequest { UserId = studentId, ResumeId = resumeId, VacancyId = vacancyId });

            // Шаг 2: Приглашение на собеседование
            await _service.UpdateApplicationStatus(new UpdateApplicationStatusRequest { UserId = employerId, ApplicationId = createdAppId, Role = Roles.Employer, ApplicationStatus = JobApplicationStatuses.InterviewInvited });

            // Шаг 3: Отказ после собеседования
            await _service.UpdateApplicationStatus(new UpdateApplicationStatusRequest { UserId = employerId, ApplicationId = createdAppId, Role = Roles.Employer, ApplicationStatus = JobApplicationStatuses.Rejected });

            // Финальный шаг: проверка статуса
            Assert.Equal((int)JobApplicationStatuses.Rejected, storedApplication!.ApplicationStatusId);
            _chatServiceMock.Verify(x => x.CloseChatByApplicationId(employerId, Roles.Employer, createdAppId), Times.Once);
        }

        [Fact]
        public async Task WithdrawalAfterOffer_Scenario()
        {
            int studentId = 1;
            int employerId = 2;
            int resumeId = 1;
            int vacancyId = 1;
            int expectedApplicationId = 103;
            int chatId = 53;

            JobApplication? storedApplication = null;

            _resumeRepoMock.Setup(x => x.IsStudentOwnsResume(studentId, resumeId)).ReturnsAsync(true);
            _vacancyRepoMock.Setup(x => x.GetVacancyById(vacancyId)).ReturnsAsync(new Vacancy { Id = vacancyId, IsActive = true, CompanyId = 1 });
            _applicationRepoMock.Setup(x => x.HasStudentActiveApplicationOnVacancy(resumeId, vacancyId)).ReturnsAsync(false);
            _chatServiceMock.Setup(x => x.GetOrCreateStudentChat(It.IsAny<StartStudentChatRequest>())).ReturnsAsync(chatId);

            _applicationRepoMock.Setup(x => x.AddJobApplication(It.IsAny<JobApplication>())).Callback<JobApplication>(app =>
            {
                app.Id = expectedApplicationId;
                storedApplication = app;
            }).Returns(Task.CompletedTask);

            _applicationRepoMock.Setup(x => x.GetEmployerApplicationForUpdate(employerId, expectedApplicationId)).ReturnsAsync(() => storedApplication);
            _applicationRepoMock.Setup(x => x.GetStudentApplicationForUpdate(studentId, expectedApplicationId)).ReturnsAsync(() => storedApplication);

            // Шаг 1: Создание отклика
            int createdAppId = await _service.CreateJobApplicationByStudent(new CreateJobApplicationRequest { UserId = studentId, ResumeId = resumeId, VacancyId = vacancyId });

            // Шаг 2: Приглашение на собеседование
            await _service.UpdateApplicationStatus(new UpdateApplicationStatusRequest { UserId = employerId, ApplicationId = createdAppId, Role = Roles.Employer, ApplicationStatus = JobApplicationStatuses.InterviewInvited });

            // Шаг 3: Оффер
            await _service.UpdateApplicationStatus(new UpdateApplicationStatusRequest { UserId = employerId, ApplicationId = createdAppId, Role = Roles.Employer, ApplicationStatus = JobApplicationStatuses.OfferReceived });

            // Шаг 4: Студент отзывает отклик
            await _service.UpdateApplicationStatus(new UpdateApplicationStatusRequest { UserId = studentId, ApplicationId = createdAppId, Role = Roles.Student, ApplicationStatus = JobApplicationStatuses.Withdrawn });

            // Финальный шаг: проверка статуса
            Assert.Equal((int)JobApplicationStatuses.Withdrawn, storedApplication!.ApplicationStatusId);
            _chatServiceMock.Verify(x => x.CloseChatByApplicationId(studentId, Roles.Student, createdAppId), Times.Once);
        }
    }
}
