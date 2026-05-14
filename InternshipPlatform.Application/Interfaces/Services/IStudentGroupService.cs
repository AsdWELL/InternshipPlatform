using InternshipPlatform.Application.Dtos.StudentGroup;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IStudentGroupService
    {
        Task<CreateStudentGroupResponse> CreateStudentGroup(CreateStudentGroupRequest request);

        Task<List<StudentGroupResponse>> GetCuratorGroups(int curatorId);

        Task<CuratorGroupDetails> GetGroupDetailsForCurator(int curatorId, int groupId);

        Task<StudentGroupDetails?> GetGroupDetailsForStudent(int studentId);

        Task<CreateStudentGroupResponse> RefreshInviteCode(int curatorId, int groupId);
    }
}
