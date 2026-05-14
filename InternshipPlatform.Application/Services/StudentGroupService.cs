using InternshipPlatform.Application.Dtos.StudentGroup;
using InternshipPlatform.Application.Exceptions.StudentGroup;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;

namespace InternshipPlatform.Application.Services
{
    public class StudentGroupService(
        IStudentGroupRepository studentGroupRepository,
        IInviteCodeGenerator inviteCodeGenerator,
        IUnitOfWork unitOfWork) : IStudentGroupService
    {
        private async Task ThrowIfCuratorDoesNotOwnGroup(int curatorId, int groupId)
        {
            if (!await studentGroupRepository.IsCuratorOwnsGroup(curatorId, groupId))
                throw new StudentGroupNotFoundException();
        }
        
        public async Task<StudentGroupInviteCodeResponse> CreateStudentGroup(CreateStudentGroupRequest request)
        {
            if (await studentGroupRepository.IsGroupExists(request.CuratorId, request.Name))
                throw new StudentGroupAlreadyExistsException(request.Name);

            var inviteCode = await inviteCodeGenerator.GenerateAsync();

            var group = request.ToDomain(inviteCode);

            await studentGroupRepository.AddStudentGroup(group);

            await unitOfWork.SaveChangesAsync();

            return new StudentGroupInviteCodeResponse
            {
                Id = group.Id,
                InviteCode = inviteCode
            };
        }

        public async Task<List<StudentGroupResponse>> GetCuratorGroups(int curatorId)
        {
            var groupsResult = await studentGroupRepository.GetCuratorGroups(curatorId);

            return groupsResult.Select(g => g.ToResponse()).ToList();
        }

        public async Task<CuratorGroupDetails> GetGroupDetailsForCurator(int curatorId, int groupId)
        {
            await ThrowIfCuratorDoesNotOwnGroup(curatorId, groupId);

            var group = await studentGroupRepository.GetGroupDetailsByGroupId(groupId)
                ?? throw new StudentGroupNotFoundException();

            return group.ToCuratorDetails();
        }

        public async Task<StudentGroupDetails?> GetGroupDetailsForStudent(int studentId)
        {
            var group = await studentGroupRepository.GetGroupDetailsForStudent(studentId);

            if (group is null)
                return null;

            return group.ToStudentDetails();
        }

        public async Task<StudentGroupInviteCodeResponse> RefreshInviteCode(int curatorId, int groupId)
        {
            await ThrowIfCuratorDoesNotOwnGroup(curatorId, groupId);

            var inviteCode = await inviteCodeGenerator.GenerateAsync();

            await studentGroupRepository.UpdateInviteCode(groupId, inviteCode);

            await unitOfWork.SaveChangesAsync();

            return new StudentGroupInviteCodeResponse
            {
                Id = groupId,
                InviteCode = inviteCode
            };
        }
    }
}
