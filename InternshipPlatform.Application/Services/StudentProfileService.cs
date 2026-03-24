using InternshipPlatform.Application.Dtos.StudentProfile;
using InternshipPlatform.Application.Exceptions.Image;
using InternshipPlatform.Application.Exceptions.StudentProfile;
using InternshipPlatform.Application.Exceptions.User;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Interfaces.Services.Auth;
using InternshipPlatform.Application.Mappers;
using InternshipPlatform.Application.Utils;
using Microsoft.AspNetCore.Http;

namespace InternshipPlatform.Application.Services
{
    public class StudentProfileService(
        IUserRepository userRepository,
        IStudentProfileRepository studentProfileRepository,
        IPasswordHasher passwordHasher,
        IImageService imageService,
        IUnitOfWork unitOfWork) : IStudentProfileService
    {
        private const int MaxAvatarSizeMb = 10;

        private async Task ThrowIfEmailAlreadyTaken(UpdateStudentProfileRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
                return;

            var normalizedEmail = StringNormalizer.NormalizeToLower(request.Email)!;

            var user = await userRepository.GetUserByEmail(normalizedEmail);

            if (user is not null && user.Id != request.UserId)
                throw new EmailAlreadyTakenException(nameof(request.Email), request.Email);
        }

        public async Task<StudentProfileResponse> GetStudentByEmail(string email)
        {
            var normalizedEmail = StringNormalizer.NormalizeToLower(email)!;

            var student = await studentProfileRepository.GetStudentByEmail(normalizedEmail)
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
            if (!await studentProfileRepository.IsStudentExists(request.UserId))
                throw new StudentProfileNotFoundException();

            await ThrowIfEmailAlreadyTaken(request);

            string? passwordHash = null;

            if (request.Password is not null)
                passwordHash = passwordHasher.Generate(request.Password);
            
            await studentProfileRepository.UpdateStudentProfile(request.ToDomain(passwordHash));

            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateStudentAvatar(int id, IFormFile avatarFile)
        {
            var student = await studentProfileRepository.GetStudentById(id)
                ?? throw new StudentProfileNotFoundException();

            int maxAvatarSizeBytes = MaxAvatarSizeMb * 1024 * 1024;

            if (avatarFile.Length == 0)
                throw new EmptyImageException();

            if (avatarFile.Length > maxAvatarSizeBytes)
                throw new MaxImageSizeException(MaxAvatarSizeMb);

            var oldAvatarPath = student.AvatarPath;

            await using var fstream = avatarFile.OpenReadStream();

            var newAvatarPath = await imageService.SaveStudentProfileAvatar(fstream, Path.GetExtension(avatarFile.FileName));
            await studentProfileRepository.UpdateAvatar(id, newAvatarPath);

            try
            {
                await unitOfWork.SaveChangesAsync();
            }
            catch
            {
                await imageService.DeleteIfExists(newAvatarPath);
                throw;
            }

            await imageService.DeleteIfExists(oldAvatarPath);
        }
    }
}
