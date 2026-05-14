using InternshipPlatform.Application.Dtos.StudentGroup;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class StudentGroupMapper
    {
        public static StudentGroup ToDomain(
            this CreateStudentGroupRequest request,
            string inviteCode,
            string groupName,
            EducationalProgram educationalProgram)
        {
            return new StudentGroup
            {
                Name = groupName,
                EducationalProgramId = request.EducationalProgramId,
                EnrollmentYear = request.EnrollmentYear,
                GraduationYear = request.EnrollmentYear + educationalProgram.DurationYears,
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
                Specialization = studentGroup.EducationalProgram.Name,
                SpecializationCode = studentGroup.EducationalProgram.SpecializationCode,
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
                Specialization = studentGroup.EducationalProgram.Name,
                SpecializationCode = studentGroup.EducationalProgram.SpecializationCode,
                InviteCode = studentGroup.InviteCode,
                EnrollmentYear = studentGroup.EnrollmentYear,
                GraduationYear = studentGroup.GraduationYear,
                StudentsCount = studentGroup.StudentProfiles.Count,
                Students = studentGroup.StudentProfiles.Select(sp => sp.ToItem()).ToList()
            };
        }
    }
}
