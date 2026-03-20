using InternshipPlatform.Application.Dtos.StudentProfile;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    public class StudentProfileController(IStudentProfileService studentProfileService) : AuthorizedUserController
    {
        [Authorize(Roles = Roles.Student)]
        [HttpGet]
        public async Task<IActionResult> GetCurrentStudentProfile()
        {
            return Ok(await studentProfileService.GetStudentById(UserId));
        }

        [Authorize(Roles = Roles.Employer)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStudentById([FromRoute] int id)
        {
            return Ok(await studentProfileService.GetStudentById(id));
        }

        [Authorize(Roles = Roles.Employer)]
        [HttpGet("{email}")]
        public async Task<IActionResult> GetStudentByEmail([FromRoute] string email)
        {
            return Ok(await studentProfileService.GetStudentByEmail(email));
        }

        [Authorize(Roles = Roles.Student)]
        [HttpPut]
        public async Task<IActionResult> UpdateStudentProfile([FromBody] UpdateStudentProfileRequest request)
        {
            request.UserId = UserId;
            
            await studentProfileService.UpdateStudentProfile(request);

            return Ok();
        }
    }
}
