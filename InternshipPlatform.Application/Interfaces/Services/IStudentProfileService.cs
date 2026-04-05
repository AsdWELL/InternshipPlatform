using InternshipPlatform.Application.Dtos.StudentProfile;
using Microsoft.AspNetCore.Http;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IStudentProfileService
    {
        Task<StudentProfileResponse> GetStudentById(int id);

        Task<StudentProfileResponse> GetStudentByEmail(string email);

        Task UpdateStudentProfile(UpdateStudentProfileRequest request);

        Task UpdateStudentAvatar(int id, IFormFile avatarFile);

        Task DeleteAvatar(int studentId);

        Task Logout(int id);

        Task DeleteStudentProfile(int id);
    }
}
