using InternshipPlatform.Application.Dtos.Auth;
using InternshipPlatform.Application.Dtos.StudentProfile;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
        [HttpGet("by-id/{id:int}")]
        public async Task<IActionResult> GetStudentById([FromRoute] int id)
        {
            return Ok(await studentProfileService.GetStudentById(id));
        }

        [Authorize(Roles = Roles.Employer)]
        [HttpGet("by-email/{email}")]
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

        [Authorize(Roles = Roles.Student)]
        [HttpPost("avatar")]
        public async Task<IActionResult> UpdateStudentAvatar(IFormFile avatarFile)
        {
            await studentProfileService.UpdateStudentAvatar(UserId, avatarFile);

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(IOptions<TokenOptions> tokenOption)
        {
            await studentProfileService.Logout(UserId);

            ClearToken(tokenOption.Value.CookieTitle);

            return Ok();
        }

        [Authorize(Roles = Roles.Student)]
        [HttpDelete]
        public async Task<IActionResult> DeleteStudentProfile(IOptions<TokenOptions> tokenOptions)
        {
            await studentProfileService.DeleteStudentProfile(UserId);

            ClearToken(tokenOptions.Value.CookieTitle);

            return Ok();
        }
    }
}
