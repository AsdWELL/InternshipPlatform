using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.Auth;
using InternshipPlatform.Application.Dtos.Teacher;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InternshipPlatform.Api.Controllers.Teachers
{
    [Authorize(Roles = Roles.Teacher)]
    [Route("api/teachers/me")]
    public class CurrentTeacherController(
        ITeacherService teacherService,
        IOptions<TokenOptions> tokenOptions) : AuthorizedUserController
    {
        [HttpGet]
        public async Task<IActionResult> GetCurrentTeacher()
        {
            return Ok(await teacherService.GetTeacherById(UserId));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTeacherProfile([FromBody] UpdateTeacherRequest request)
        {
            request.UserId = UserId;
            
            await teacherService.UpdateTeacherProfile(request);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTeacher()
        {
            await teacherService.DeleteTeacher(UserId);

            ClearToken(tokenOptions.Value.CookieTitle);

            return Ok();
        }

        [HttpPut("avatar")]
        public async Task<IActionResult> UpdateTeacherAvatar(IFormFile avatarFile)
        {
            await teacherService.UpdateTeacherAvatar(UserId, avatarFile);

            return Ok();
        }

        [HttpDelete("avatar")]
        public async Task<IActionResult> DeleteAvatar()
        {
            await teacherService.DeleteAvatar(UserId);

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await teacherService.Logout(UserId);

            ClearToken(tokenOptions.Value.CookieTitle);

            return Ok();
        }
    }
}
