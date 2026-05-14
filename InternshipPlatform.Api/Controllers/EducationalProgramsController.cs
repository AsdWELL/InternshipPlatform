using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers
{
    [Authorize(Roles = Roles.Teacher)]
    [Route("api/educational-programs")]
    public class EducationalProgramsController(IEducationalProgramsService educationalProgramsService) : AuthorizedUserController
    {
        [HttpGet]
        public async Task<IActionResult> SearchEducationalPrograms([FromQuery] string? name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                return Ok(await educationalProgramsService.SearchUniversityEducationalPrograms(UserId, name));

            return Ok(await educationalProgramsService.GetAllUniversityEducationalPrograms(UserId));
        }
    }
}
