using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface ISkillRepository
    {
        Task<List<Skill>> GetSkillsByIds(IEnumerable<int> ids);

        Task<List<Skill>> GetAllSkills();

        Task<List<Skill>> SearchSkills(string skillName);
    }
}
