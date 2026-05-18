using InternshipPlatform.Application.Dtos.StudentGroup;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Utils;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class StudentGroupRepository(InternshipPlatformContext context) : IStudentGroupRepository
    {
        public Task<bool> IsGroupExists(int curatorId, string groupName)
        {
            var normalizedGroupName = groupName.Trim().ToUpperInvariant();

            return context.StudentGroups
                .AsNoTracking()
                .AnyAsync(g => g.UniversityId == 
                    context.Teachers
                        .AsNoTracking()
                        .Where(c => c.UserId == curatorId)
                        .Select(c => c.UniversityId)
                        .FirstOrDefault()
                    && g.Name == normalizedGroupName);
        }

        public Task<bool> IsCuratorOwnsGroup(int curatorId, int groupId)
        {
            return context.StudentGroups
                .AsNoTracking()
                .AnyAsync(g => g.Id == groupId && g.CuratorId == curatorId);
        }

        public async Task<int> GetNextGroupNumber(int universityId, int educationalProgramId, int enrollmentYear)
        {
            var maxGroupNumber = await context.StudentGroups
                .AsNoTracking()
                .Where(g =>
                    g.UniversityId == universityId &&
                    g.EducationalProgramId == educationalProgramId &&
                    g.EnrollmentYear == enrollmentYear)
                .CountAsync();

            return maxGroupNumber + 1;
        }

        public async Task AddStudentGroup(StudentGroup studentGroup)
        {
            var universityId = await context.Teachers
                .Where(c => c.UserId == studentGroup.CuratorId)
                .Select(c => c.UniversityId)
                .FirstOrDefaultAsync();
            
            studentGroup.UniversityId = universityId;
            
            await context.StudentGroups.AddAsync(studentGroup);
        }

        public Task<List<StudentGroupResult>> GetCuratorGroups(int curatorId)
        {
            return context.StudentGroups
                .AsNoTracking()
                .Where(g => g.CuratorId == curatorId)
                .Select(g => new StudentGroupResult
                {
                    StudentGroup = g,
                    StudentsCount = g.StudentProfiles.Count()
                })
                .ToListAsync();
        }

        public Task<StudentGroup?> GetGroupDetailsByGroupId(int groupId)
        {
            return context.StudentGroups
                .AsNoTracking()
                .Include(g => g.StudentProfiles)
                    .ThenInclude(sp => sp.User)
                .Include(g => g.EducationalProgram)
                .FirstOrDefaultAsync(g => g.Id == groupId);
        }

        public Task<StudentGroup?> GetGroupDetailsForStudent(int studentId)
        {
            return context.StudentProfiles
                .AsNoTracking()
                .Where(sp => sp.UserId == studentId && sp.GroupId != null)
                .Include(sp => sp.Group!)
                    .ThenInclude(g => g.StudentProfiles)
                .Include(sp => sp.Group!)
                    .ThenInclude(g => g.Curator)
                        .ThenInclude(c => c.User)
                .Include(sp => sp.Group!)
                    .ThenInclude(g => g.EducationalProgram)
                .Select(sp => sp.Group!)
                .FirstOrDefaultAsync();
        }

        public Task<int?> GetGroupIdByInviteCode(string inviteCode)
        {
            var normalizedCode = StringNormalizer.NormalizeToUpper(inviteCode)!;
            
            return context.StudentGroups
                .AsNoTracking()
                .Where(g => g.InviteCode == normalizedCode)
                .Select(g => (int?)g.Id)
                .FirstOrDefaultAsync();
        }

        public Task<bool> IsInviteCodeExists(string inviteCode)
        {
            return context.StudentGroups
                .AsNoTracking()
                .AnyAsync(g => g.InviteCode == inviteCode);
        }

        public async Task UpdateInviteCode(int groupId, string inviteCode)
        {
            var group = await context.StudentGroups.FindAsync(groupId);

            if (group == null)
                return;

            group.InviteCode = inviteCode;
        }
    }
}
