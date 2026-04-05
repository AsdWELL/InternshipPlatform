using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class SkillRepository(InternshipPlatformContext context) : ISkillRepository
    {
        public async Task<List<Skill>> GetSkillsByIds(IEnumerable<int> ids)
        {
            return await context.Skills
                .Where(skill => ids.Contains(skill.Id))
                .ToListAsync();
        }

        public async Task<List<Skill>> GetAllSkills()
        {
            return await context.Skills
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Skill>> SearchSkills(string skillName)
        {
            var search = skillName.Trim().ToLowerInvariant();
            
            return await context.Skills
                .AsNoTracking()
                .Where(skill => skill.Name.ToLower().Contains(search))
                .ToListAsync();
        }
    }
}
