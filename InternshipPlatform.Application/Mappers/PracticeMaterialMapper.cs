using InternshipPlatform.Application.Dtos.File;
using InternshipPlatform.Application.Dtos.PracticeMaterial;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class PracticeMaterialMapper
    {
        public static PracticeMaterial ToPracticeMaterial(
            this SavedFileResult savedFileResult,
            int practiceOfferId)
        {
            return new PracticeMaterial
            {
                PracticeOfferId = practiceOfferId,
                FileName = savedFileResult.OriginalFileName,
                FilePath = savedFileResult.RelativePath
            };
        }
        
        public static PracticeMaterialResponse ToResponse(this PracticeMaterial material)
        {
            return new PracticeMaterialResponse
            {
                Id = material.Id,
                FileName = material.FileName
            };
        }
    }
}
