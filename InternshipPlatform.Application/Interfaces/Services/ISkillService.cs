using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface ISkillService
    {
        Task<List<Skill>> GetAllSkills();

        Task<List<Skill>> SearchSkills(string skillName);
    }
}
