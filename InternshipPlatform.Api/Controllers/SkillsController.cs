using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers
{
    [Route("api/skills")]
    public class SkillsController(ISkillService skillService) : AuthorizedUserController
    {
        [HttpGet]
        public async Task<IActionResult> SearchSkills([FromQuery] string? skillName)
        {
            if (!string.IsNullOrWhiteSpace(skillName))
                return Ok(await skillService.SearchSkills(skillName));

            return Ok(await skillService.GetAllSkills());
        }
    }
}
