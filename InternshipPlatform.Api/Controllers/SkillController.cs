using InternshipPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    public class SkillController(ISkillService skillService) : AuthorizedUserController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllSkills()
        {
            return Ok(await skillService.GetAllSkills());
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchSkills([FromQuery] string skillName)
        {
            return Ok(await skillService.SearchSkills(skillName));
        }
    }
}
