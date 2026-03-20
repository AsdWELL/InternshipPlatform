using InternshipPlatform.Application.Dtos.StudentProfile;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IStudentProfileService
    {
        Task<StudentProfileResponse> GetStudentById(int id);

        Task<StudentProfileResponse> GetStudentByEmail(string email);

        Task UpdateStudentProfile(UpdateStudentProfileRequest request);
    }
}
