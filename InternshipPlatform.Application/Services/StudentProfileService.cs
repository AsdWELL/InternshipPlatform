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
using InternshipPlatform.Domain.Entities;
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

        private async Task ThrowIfStudentProfileNotExists(int userId)
        {
            if (!await studentProfileRepository.IsStudentExists(userId))
                throw new StudentProfileNotFoundException();
        }
        
        private async Task ThrowIfEmailAlreadyTaken(UpdateStudentProfileRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
                return;

            var normalizedEmail = StringNormalizer.NormalizeToLower(request.Email)!;

            var user = await userRepository.GetUserByEmail(normalizedEmail);

            if (user is not null && user.Id != request.UserId)
                throw new EmailAlreadyTakenException(request.Email);
        }

        private async Task<StudentProfile> GetStudentByIdOrThrow(int id)
        {
            var student = await studentProfileRepository.GetStudentById(id)
                ?? throw new StudentProfileNotFoundException();

            return student;
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
            var student = await GetStudentByIdOrThrow(id);

            return student.ToResponse();
        }

        public async Task UpdateStudentProfile(UpdateStudentProfileRequest request)
        {
            var student = await studentProfileRepository.GetStudentForUpdate(request.UserId)
                ?? throw new StudentProfileNotFoundException();

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                await ThrowIfEmailAlreadyTaken(request);

                student.User.Email = StringNormalizer.NormalizeToLower(request.Email)!;
            }

            if (!string.IsNullOrWhiteSpace(request.Password))
                student.User.PasswordHash = passwordHasher.Generate(request.Password);

            if (!string.IsNullOrWhiteSpace(request.Name))
                student.Name = StringNormalizer.NormalizeName(request.Name)!;

            if (!string.IsNullOrWhiteSpace(request.Surname))
                student.Surname = StringNormalizer.NormalizeName(request.Surname)!;

            if (request.Patronymic is not null)
                student.Patronymic = StringNormalizer.NormalizeName(request.Patronymic);

            if (request.BirthdayDate is not null)
                student.BirthdayDate = request.BirthdayDate;

            if (request.Phone is not null)
                student.Phone = StringNormalizer.NormalizePhone(request.Phone);

            if (request.VkLink is not null)
                student.VkLink = StringNormalizer.NormalizeToLower(request.VkLink);

            if (request.TgLink is not null)
                student.TgLink = StringNormalizer.NormalizeToLower(request.TgLink);

            if (request.GithubLink is not null)
                student.GithubLink = StringNormalizer.NormalizeToLower(request.GithubLink);

            if (request.MaxLink is not null)
                student.MaxLink = StringNormalizer.NormalizeToLower(request.MaxLink);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateStudentAvatar(int id, IFormFile avatarFile)
        {
            var student = await GetStudentByIdOrThrow(id);

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

        public async Task DeleteAvatar(int studentId)
        {
            var student = await GetStudentByIdOrThrow(studentId);

            await studentProfileRepository.UpdateAvatar(studentId, null);
            await imageService.DeleteIfExists(student.AvatarPath);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task Logout(int id)
        {
            await userRepository.UpdateRefreshToken(id, null, DateTime.UtcNow);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteStudentProfile(int id)
        {
            await ThrowIfStudentProfileNotExists(id);

            await userRepository.DeleteUserById(id);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
