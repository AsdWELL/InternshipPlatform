using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.Auth;
using InternshipPlatform.Application.Dtos.EmployerProflie;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InternshipPlatform.Api.Controllers
{
    [Authorize(Roles = Roles.Employer)]
    [Route("api/employers/me")]
    public class CurrentEmployerController(
        IEmployerProfileService employerProfileService,
        IOptions<TokenOptions> tokenOptions) : AuthorizedUserController
    {
        [HttpGet]
        public async Task<IActionResult> GetCurrentEmployerProfile()
        {
            return Ok(await employerProfileService.GetEmployerProfileById(UserId));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployerProfile([FromBody] UpdateEmployerProfileRequest request)
        {
            request.UserId = UserId;

            await employerProfileService.UpdateEmployerProfile(request);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployerProfile()
        {
            await employerProfileService.DeleteEmployerProfile(UserId);

            ClearToken(tokenOptions.Value.CookieTitle);

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await employerProfileService.Logout(UserId);

            ClearToken(tokenOptions.Value.CookieTitle);

            return Ok();
        }
    }
}
