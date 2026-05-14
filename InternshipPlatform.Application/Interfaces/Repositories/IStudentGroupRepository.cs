using InternshipPlatform.Application.Dtos.StudentGroup;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IStudentGroupRepository
    {
        Task<bool> IsGroupExists(int curatorId, string groupName);

        Task<bool> IsCuratorOwnsGroup(int curatorId, int groupId);

        Task<int> GetNextGroupNumber(int universityId, int educationalProgramId, int enrollmentYear);
        
        Task AddStudentGroup(StudentGroup studentGroup);

        Task<List<StudentGroupResult>> GetCuratorGroups(int curatorId);

        Task<StudentGroup?> GetGroupDetailsByGroupId(int groupId);

        Task<StudentGroup?> GetGroupDetailsForStudent(int studentId);

        Task<int?> GetGroupIdByInviteCode(string inviteCode);

        Task<bool> IsInviteCodeExists(string inviteCode);

        Task UpdateInviteCode(int groupId, string inviteCode);
    }
}
