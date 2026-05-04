using InternshipPlatform.Application.Dtos.StudentGroup;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IStudentGroupService
    {
        Task<StudentGroupInviteCodeResponse> CreateStudentGroup(CreateStudentGroupRequest request);

        Task<List<StudentGroupResponse>> GetCuratorGroups(int curatorId);

        Task<StudentGroupDetails> GetGroupDetailsForCurator(int curatorId, int groupId);

        Task<StudentGroupDetails?> GetGroupDetailsForStudent(int studentId);

        Task<StudentGroupInviteCodeResponse> RefreshInviteCode(int curatorId, int groupId);
    }
}
