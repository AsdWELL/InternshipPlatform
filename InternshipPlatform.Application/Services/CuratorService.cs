using InternshipPlatform.Application.Dtos.Curator;
using InternshipPlatform.Application.Exceptions.Curator;
using InternshipPlatform.Application.Exceptions.Image;
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
    public class CuratorService(
        ICuratorRepository curatorRepository,
        IUserRepository userRepository,
        IImageService imageService,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork) : ICuratorService
    {
        private const int MaxAvatarSizeMb = 10;

        private async Task ThrowIfCuratorNotExists(int id)
        {
            if (!await curatorRepository.IsCuratorExists(id))
                throw new CuratorNotFoundException();
        }

        private async Task ThrowIfEmailAlreadyTaken(UpdateCuratorRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
                return;

            var normalizedEmail = StringNormalizer.NormalizeToLower(request.Email)!;

            var user = await userRepository.GetUserByEmail(normalizedEmail);

            if (user is not null && user.Id != request.UserId)
                throw new EmailAlreadyTakenException(request.Email);
        }

        private async Task<Curator> GetCuratorByIdOrThrow(int id)
        {
            var curator = await curatorRepository.GetCuratorById(id)
                ?? throw new CuratorNotFoundException();

            return curator;
        }

        public async Task<CuratorResponse> GetCuratorById(int id)
        {
            var curator = await GetCuratorByIdOrThrow(id);

            return curator.ToResponse();
        }

        public async Task UpdateCuratorProfile(UpdateCuratorRequest request)
        {
            var curator = await curatorRepository.GetCuratorForUpdate(request.UserId)
                ?? throw new CuratorNotFoundException();

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                await ThrowIfEmailAlreadyTaken(request);

                curator.User.Email = StringNormalizer.NormalizeToLower(request.Email)!;
            }

            if (!string.IsNullOrWhiteSpace(request.Password))
                curator.User.PasswordHash = passwordHasher.Generate(request.Password);

            if (!string.IsNullOrWhiteSpace(request.Name))
                curator.Name = StringNormalizer.NormalizeName(request.Name)!;

            if (!string.IsNullOrWhiteSpace(request.Surname))
                curator.Surname = StringNormalizer.NormalizeName(request.Surname)!;

            if (request.Patronymic is not null)
                curator.Patronymic = StringNormalizer.NormalizeName(request.Patronymic);

            if (request.Phone is not null)
                curator.Phone = StringNormalizer.NormalizePhone(request.Phone);

            if (request.VkLink is not null)
                curator.VkLink = StringNormalizer.NormalizeToLower(request.VkLink);

            if (request.TgLink is not null)
                curator.TgLink = StringNormalizer.NormalizeToLower(request.TgLink);

            if (request.MaxLink is not null)
                curator.MaxLink = StringNormalizer.NormalizeToLower(request.MaxLink);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateCuratorAvatar(int id, IFormFile avatarFile)
        {
            var curator = await GetCuratorByIdOrThrow(id);

            int maxAvatarSizeBytes = MaxAvatarSizeMb * 1024 * 1024;

            if (avatarFile.Length == 0)
                throw new EmptyImageException();

            if (avatarFile.Length > maxAvatarSizeBytes)
                throw new MaxImageSizeException(MaxAvatarSizeMb);

            var oldAvatarPath = curator.AvatarPath;

            await using var fstream = avatarFile.OpenReadStream();

            var newAvatarPath = await imageService.SaveCuratorProfileAvatar(fstream, Path.GetExtension(avatarFile.FileName));
            await curatorRepository.UpdateAvatar(id, newAvatarPath);

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

        public async Task DeleteAvatar(int id)
        {
            var student = await GetCuratorByIdOrThrow(id);

            await curatorRepository.UpdateAvatar(id, null);
            await imageService.DeleteIfExists(student.AvatarPath);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task Logout(int id)
        {
            await userRepository.UpdateRefreshToken(id, null, DateTime.UtcNow);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCurator(int id)
        {
            await ThrowIfCuratorNotExists(id);

            await userRepository.DeleteUserById(id);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
