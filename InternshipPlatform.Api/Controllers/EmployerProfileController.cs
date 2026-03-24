using InternshipPlatform.Application.Dtos.EmployerProflie;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers
{
    [Authorize(Roles = Roles.Employer)]
    [Route("api/[controller]")]
    public class EmployerProfileController(IEmployerProflieService employerProflieService) : AuthorizedUserController
    {
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
    }
}
