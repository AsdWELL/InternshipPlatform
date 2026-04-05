using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Services
{
    public class SkillService(ISkillRepository skillRepository) : ISkillService
    {
        public Task<List<Skill>> GetAllSkills()
        {
            return skillRepository.GetAllSkills();
        }

        public Task<List<Skill>> SearchSkills(string skillName)
        {
            return skillRepository.SearchSkills(skillName);
        }
    }
}
