using InternshipPlatform.Application.Dtos.StudentGroup;
using InternshipPlatform.Application.Utils;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class StudentGroupMapper
    {
        public static StudentGroup ToDomain(
            this CreateStudentGroupRequest request,
            string inviteCode)
        {
            return new StudentGroup
            {
                Name = StringNormalizer.NormalizeToUpper(request.Name)!,
                Specialization = StringNormalizer.NormalizeName(request.Specialization)!,
                EnrollmentYear = request.EnrollmentYear,
                GraduationYear = request.GraduationYear,
                InviteCode = inviteCode,
                CuratorId = request.CuratorId
            };
        }

        public static StudentGroupResponse ToResponse(this StudentGroupResult result)
        {
            var studentGroup = result.StudentGroup;

            return new StudentGroupResponse
            {
                Id = studentGroup.Id,
                Name = studentGroup.Name,
                EnrollmentYear = studentGroup.EnrollmentYear,
                GraduationYear = studentGroup.GraduationYear,
                StudentsCount = result.StudentsCount
            };
        }

        public static CuratorGroupDetails ToCuratorDetails(this StudentGroup studentGroup)
        {
            return new CuratorGroupDetails
            {
                Id = studentGroup.Id,
                Name = studentGroup.Name,
                Specialization = studentGroup.Specialization,
                InviteCode = studentGroup.InviteCode,
                EnrollmentYear = studentGroup.EnrollmentYear,
                GraduationYear = studentGroup.GraduationYear,
                StudentsCount = studentGroup.StudentProfiles.Count,
                Students = studentGroup.StudentProfiles.Select(sp => sp.ToItem()).ToList()
            };
        }

        public static StudentGroupDetails ToStudentDetails(this StudentGroup studentGroup)
        {
            return new StudentGroupDetails
            {
                Id = studentGroup.Id,
                CuratorId = studentGroup.CuratorId,
                CuratorName = studentGroup.Curator.Name,
                CuratorSurname = studentGroup.Curator.Surname,
                CuratorPatronymic = studentGroup.Curator.Patronymic,
                CuratorAvatarPath = studentGroup.Curator.AvatarPath,
                Name = studentGroup.Name,
                Specialization = studentGroup.Specialization,
                InviteCode = studentGroup.InviteCode,
                EnrollmentYear = studentGroup.EnrollmentYear,
                GraduationYear = studentGroup.GraduationYear,
                StudentsCount = studentGroup.StudentProfiles.Count,
                Students = studentGroup.StudentProfiles.Select(sp => sp.ToItem()).ToList()
            };
        }
    }
}
