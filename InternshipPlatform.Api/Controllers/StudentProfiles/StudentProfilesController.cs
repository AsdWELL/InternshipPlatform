using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.StudentProfiles
{
    [Authorize(Roles = Roles.Employer)]
    [Route("api/students")]
    public class StudentProfilesController(IStudentProfileService studentProfileService) : AuthorizedUserController
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStudentById([FromRoute] int id)
        {
            return Ok(await studentProfileService.GetStudentById(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentByEmail([FromQuery] string email)
        {
            return Ok(await studentProfileService.GetStudentByEmail(email));
        }
    }
}
