using InternshipPlatform.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class UnitOfWork(InternshipPlatformContext context) : IUnitOfWork
    {
        public Task SaveChangesAsync(CancellationToken ct = default)
        {
            return context.SaveChangesAsync(ct);
        }
    }
}
