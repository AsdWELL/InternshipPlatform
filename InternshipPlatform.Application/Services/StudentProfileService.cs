using InternshipPlatform.Application.Dtos.StudentProfile;
using InternshipPlatform.Application.Exceptions.StudentProfile;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Interfaces.Services.Auth;
using InternshipPlatform.Application.Mappers;

namespace InternshipPlatform.Application.Services
{
    public class StudentProfileService(
        IStudentProfileRepository studentProfileRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork) : IStudentProfileService
    {
        public async Task<StudentProfileResponse> GetStudentByEmail(string email)
        {
            var student = await studentProfileRepository.GetStudentByEmail(email)
                ?? throw new StudentProfileNotFoundException();

            return student.ToResponse();
        }

        public async Task<StudentProfileResponse> GetStudentById(int id)
        {
            var student = await studentProfileRepository.GetStudentById(id)
                ?? throw new StudentProfileNotFoundException();

            return student.ToResponse();
        }

        public async Task UpdateStudentProfile(UpdateStudentProfileRequest request)
        {
            string? passwordHash = null;

            if (request.Password is not null)
                passwordHash = passwordHasher.Generate(request.Password);
            
            await studentProfileRepository.UpdateStudentProfile(request.ToDomain(passwordHash));

            await unitOfWork.SaveChangesAsync();
        }
    }
}
