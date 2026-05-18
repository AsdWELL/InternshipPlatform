using InternshipPlatform.Application.Exceptions.PracticeMaterial;
using InternshipPlatform.Application.Exceptions.PracticeOffer;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;
using InternshipPlatform.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace InternshipPlatform.Application.Services
{
    public class PracticeMaterialService(
        IPracticeOfferRepository practiceOfferRepository,
        IPracticeMaterialRepository practiceMaterialRepository,
        IFileStorageService fileStorageService,
        IUnitOfWork unitOfWork
        ) : IPracticeMaterialService
    {
        private async Task ThrowIfEmployerDoesNotOwnPracticeOffer(int employerId, int practiceOfferId)
        {
            if (!await practiceOfferRepository.IsEmployerOwnsPracticeOffer(employerId, practiceOfferId))
                throw new PracticeOfferNotFoundException();
        }

        public async Task UploadMaterials(int employerId, int practiceOfferId, IEnumerable<IFormFile> files)
        {
            await ThrowIfEmployerDoesNotOwnPracticeOffer(employerId, practiceOfferId);

            var fileList = files.ToList();

            if (fileList.Count == 0)
                return;

            var materials = new List<PracticeMaterial>();

            try
            {
                foreach (var file in fileList)
                {
                    if (file.Length == 0)
                        continue;
                    
                    await using var fstream = file.OpenReadStream();

                    var savedMaterial = await fileStorageService.SavePracticeMaterial(
                        fstream, file.FileName, Path.GetExtension(file.FileName));

                    materials.Add(savedMaterial.ToPracticeMaterial(practiceOfferId));
                }

                if (materials.Count == 0)
                    return;

                await practiceMaterialRepository.AddMaterials(materials);

                await unitOfWork.SaveChangesAsync();
            }
            catch
            {
                foreach (var material in materials)
                    await fileStorageService.DeleteFileIfExists(material.FilePath);

                throw;
            }
        }

        public async Task DeleteFile(int employerId, int practiceOfferId, int fileId)
        {
            await ThrowIfEmployerDoesNotOwnPracticeOffer(employerId, practiceOfferId);

            var material = await practiceMaterialRepository.GetPracticeMaterialById(fileId)
                ?? throw new PracticeMaterialNotFoundException();

            if (material.PracticeOfferId != practiceOfferId)
                throw new PracticeMaterialNotFoundException();

            await practiceMaterialRepository.DeletePracticeMaterial(material);
            await unitOfWork.SaveChangesAsync();

            await fileStorageService.DeleteFileIfExists(material.FilePath);
        }
    }
}
