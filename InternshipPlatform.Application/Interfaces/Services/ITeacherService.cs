using InternshipPlatform.Application.Dtos.Teacher;
using Microsoft.AspNetCore.Http;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface ITeacherService
    {
        Task<TeacherResponse> GetTeacherById(int id);

        Task UpdateTeacherProfile(UpdateTeacherRequest request);

        Task UpdateTeacherAvatar(int id, IFormFile avatarFile);

        Task DeleteAvatar(int id);

        Task Logout(int id);

        Task DeleteTeacher(int id);
    }
}
