using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.Teachers
{
    [Route("api/teachers")]
    public class TeacherProfilesController(ITeacherService teacherService) : AuthorizedUserController
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTeacherById([FromRoute] int id)
        {
            return Ok(await teacherService.GetTeacherById(id));
        }
    }
}
