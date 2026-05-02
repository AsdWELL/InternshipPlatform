using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.StudentProfiles
{
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
