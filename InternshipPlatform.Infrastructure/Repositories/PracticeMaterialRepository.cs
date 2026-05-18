using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class PracticeMaterialRepository(InternshipPlatformContext context) : IPracticeMaterialRepository
    {
        public async Task AddMaterials(IEnumerable<PracticeMaterial> materials)
        {
            await context.AddRangeAsync(materials);
        }

        public Task<PracticeMaterial?> GetPracticeMaterialById(int materialId)
        {
            return context.PracticeMaterials
                .AsNoTracking()
                .FirstOrDefaultAsync(pm => pm.Id == materialId);
        }

        public Task DeletePracticeMaterial(PracticeMaterial practiceMaterial)
        {
            context.PracticeMaterials.Remove(practiceMaterial);

            return Task.CompletedTask;
        }
    }
}
