using InternshipPlatform.Application.Dtos.StudentGroupApplication;
using InternshipPlatform.Application.Exceptions.StudentGroup;
using InternshipPlatform.Application.Exceptions.StudentGroupApplication;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Services
{
    public class StudentGroupApplicationService(
        IStudentProfileRepository studentProfileRepository,
        IStudentGroupApplicationRepository studentGroupApplicationRepository,
        IStudentGroupRepository studentGroupRepository,
        IUnitOfWork unitOfWork) : IStudentGroupApplicationService
    {
        public async Task<int> CreateStudentGroupApplication(CreateStudentGroupApplicationRequest request)
        {
            if (await studentProfileRepository.IsStudentHasGroup(request.StudentId))
                throw new StudentAlreadyHasGroupException();
            
            if (await studentGroupApplicationRepository.IsStudentHasGroupApplication(request.StudentId))
                throw new StudentAlreadyHasGroupApplicationException();
            
            var groupId = await studentGroupRepository.GetGroupIdByInviteCode(request.InviteCode)
                ?? throw new InviteCodeNotFoundException();

            var studentGroupApplication = new StudentGroupApplication
            {
                StudentId = request.StudentId,
                GroupId = groupId,
                CreatedAt = DateTime.UtcNow
            };

            await studentGroupApplicationRepository.AddStudentGroupApplication(studentGroupApplication);

            await unitOfWork.SaveChangesAsync();

            return studentGroupApplication.Id;
        }

        public async Task<List<StudentGroupApplicationCuratorItem>> GetCuratorGroupApplications(int curatorId)
        {
            var curatorGroupApplications = await studentGroupApplicationRepository.GetCuratorGroupApplications(curatorId);

            return curatorGroupApplications.Select(ga => ga.ToCuratorItem()).ToList();
        }

        public async Task<StudentGroupApplicationItem?> GetStudentGroupApplication(int studentId)
        {
            var groupApplication = await studentGroupApplicationRepository.GetStudentGroupApplication(studentId);

            if (groupApplication is null)
                return null;

            return groupApplication.ToItem();
        }

        public async Task DeleteStudentGroupApplication(int studentId, int applicationId)
        {
            if (!await studentGroupApplicationRepository.IsStudentOwnsGroupApplication(studentId, applicationId))
                throw new StudentGroupApplicationNotFoundException();

            await studentGroupApplicationRepository.DeleteStudentGroupApplicationById(applicationId);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task AcceptGroupApplication(int curatorId, int applicationId)
        {
            var application = await studentGroupApplicationRepository.GetCuratorGroupApplicationForUpdate(curatorId, applicationId)
                ?? throw new StudentGroupApplicationNotFoundException();

            await studentGroupApplicationRepository.DeleteStudentGroupApplication(application);

            var student = application.StudentProfile;

            if (student.GroupId is not null)
                throw new StudentAlreadyHasGroupException();

            student.GroupId = application.GroupId;

            await unitOfWork.SaveChangesAsync();
        }

        public async Task RejectGroupApplication(int curatorId, int applicationId)
        {
            var application = await studentGroupApplicationRepository.GetCuratorGroupApplicationForUpdate(curatorId, applicationId)
                ?? throw new StudentGroupApplicationNotFoundException();

            await studentGroupApplicationRepository.DeleteStudentGroupApplication(application);

            await unitOfWork.SaveChangesAsync();
        }

    }
}
