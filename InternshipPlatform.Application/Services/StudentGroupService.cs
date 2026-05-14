using InternshipPlatform.Application.Dtos.StudentGroup;
using InternshipPlatform.Application.Exceptions.EducationalProgram;
using InternshipPlatform.Application.Exceptions.StudentGroup;
using InternshipPlatform.Application.Exceptions.Teacher;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;

namespace InternshipPlatform.Application.Services
{
    public class StudentGroupService(
        IStudentGroupRepository studentGroupRepository,
        IEducationalProgramsRepository educationalProgramsRepository,
        ITeacherRepository teacherRepository,
        IInviteCodeGenerator inviteCodeGenerator,
        IUnitOfWork unitOfWork) : IStudentGroupService
    {
        private async Task ThrowIfCuratorDoesNotOwnGroup(int curatorId, int groupId)
        {
            if (!await studentGroupRepository.IsCuratorOwnsGroup(curatorId, groupId))
                throw new StudentGroupNotFoundException();
        }
        
        public async Task<CreateStudentGroupResponse> CreateStudentGroup(CreateStudentGroupRequest request)
        {
            var educationalProgram = await educationalProgramsRepository
                .GetEducationalProgramById(request.EducationalProgramId)
                ?? throw new EducationalProgramNotFoundException();

            var curator = await teacherRepository.GetTeacherById(request.CuratorId)
                ?? throw new TeacherNotFoundException();

            if (educationalProgram.UniversityId != curator.UniversityId)
                throw new EducationalProgramNotFoundException();

            var groupNumber = await studentGroupRepository.GetNextGroupNumber(
                educationalProgram.UniversityId, educationalProgram.Id, request.EnrollmentYear);

            var groupName = $"{request.EnrollmentYear.ToString()[^2..]}" +
                $"{educationalProgram.GroupCode}{groupNumber}".ToUpperInvariant();

            var inviteCode = await inviteCodeGenerator.GenerateAsync();

            var group = request.ToDomain(inviteCode, groupName, educationalProgram);

            await studentGroupRepository.AddStudentGroup(group);

            await unitOfWork.SaveChangesAsync();

            return new CreateStudentGroupResponse
            {
                Id = group.Id,
                GroupName = groupName,
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

        public async Task<CreateStudentGroupResponse> RefreshInviteCode(int curatorId, int groupId)
        {
            await ThrowIfCuratorDoesNotOwnGroup(curatorId, groupId);

            var inviteCode = await inviteCodeGenerator.GenerateAsync();

            await studentGroupRepository.UpdateInviteCode(groupId, inviteCode);

            await unitOfWork.SaveChangesAsync();

            return new CreateStudentGroupResponse
            {
                Id = groupId,
                InviteCode = inviteCode
            };
        }
    }
}
