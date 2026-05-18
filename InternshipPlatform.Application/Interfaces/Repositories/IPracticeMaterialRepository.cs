using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IPracticeMaterialRepository
    {
        Task AddMaterials(IEnumerable<PracticeMaterial> materials);

        Task<PracticeMaterial?> GetPracticeMaterialById(int materialId);

        Task DeletePracticeMaterial(PracticeMaterial practiceMaterial);
    }
}
