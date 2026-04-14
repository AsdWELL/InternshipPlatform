using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.Auth;
using InternshipPlatform.Application.Dtos.StudentProfile;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InternshipPlatform.Api.Controllers.StudentProfiles
{
    [Authorize(Roles = Roles.Student)]
    [Route("api/students/me")]
    public class CurrentStudentController(
        IStudentProfileService studentProfileService,
        IOptions<TokenOptions> tokenOptions) : AuthorizedUserController
    {
        [HttpGet]
        public async Task<IActionResult> GetCurrentStudentProfile()
        {
            return Ok(await studentProfileService.GetStudentById(UserId));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudentProfile([FromBody] UpdateStudentProfileRequest request)
        {
            request.UserId = UserId;

            await studentProfileService.UpdateStudentProfile(request);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteStudentProfile()
        {
            await studentProfileService.DeleteStudentProfile(UserId);

            ClearToken(tokenOptions.Value.CookieTitle);

            return Ok();
        }

        [HttpPut("avatar")]
        public async Task<IActionResult> UpdateStudentAvatar(IFormFile avatarFile)
        {
            await studentProfileService.UpdateStudentAvatar(UserId, avatarFile);

            return Ok();
        }

        [HttpDelete("avatar")]
        public async Task<IActionResult> DeleteAvatar()
        {
            await studentProfileService.DeleteAvatar(UserId);

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await studentProfileService.Logout(UserId);

            ClearToken(tokenOptions.Value.CookieTitle);

            return Ok();
        }
    }
}
