using InternshipPlatform.Application.Dtos.StudentGroupApplication;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class StudentGroupApplicationMapper
    {
        public static StudentGroupApplicationItem ToItem(this StudentGroupApplication studentGroupApplication)
        {
            return new StudentGroupApplicationItem
            {
                Id = studentGroupApplication.Id,
                University = studentGroupApplication.Group.University.Name,
                GroupName = studentGroupApplication.Group.Name,
                Specialization = studentGroupApplication.Group.Specialization,
                InviteCode = studentGroupApplication.Group.InviteCode,
                CreatedAt = studentGroupApplication.CreatedAt
            };
        }

        public static StudentGroupApplicationCuratorItem ToCuratorItem(this StudentGroupApplication studentGroupApplication)
        {
            return new StudentGroupApplicationCuratorItem
            {
                Id = studentGroupApplication.Id,
                Student = studentGroupApplication.StudentProfile.ToItem(),
                CreatedAt = studentGroupApplication.CreatedAt,
                GroupId = studentGroupApplication.GroupId,
                GroupName = studentGroupApplication.Group.Name
            };
        }
    }
}
