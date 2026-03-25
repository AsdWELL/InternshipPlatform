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
    [Route("api/[controller]")]
    public class EmployerProfileController(IEmployerProflieService employerProflieService) : AuthorizedUserController
    {
        [NonAction]
        private void ClearToken(string cookieTitle)
        {
            Response.Cookies.Delete(cookieTitle);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployerProfile()
        {
            return Ok(await employerProflieService.GetEmployerProfileById(UserId));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployerProfile([FromBody] UpdateEmployerProflieRequest request)
        {
            request.UserId = UserId;

            await employerProflieService.UpdateEmployerProfile(request);

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(IOptions<TokenOptions> tokenOption)
        {
            await employerProflieService.Logout(UserId);

            ClearToken(tokenOption.Value.CookieTitle);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployerProfile(IOptions<TokenOptions> tokenOptions)
        {
            await employerProflieService.DeleteEmployerProfile(UserId);

            ClearToken(tokenOptions.Value.CookieTitle);

            return Ok();
        }
    }
}
