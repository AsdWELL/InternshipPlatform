using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers
{
    [Route("api/specializations")]
    public class SpecializationsController(ISpecializationService specializationService) : AuthorizedUserController
    {
        [HttpGet]
        public async Task<IActionResult> SearchSpecialization([FromQuery] string? specializationName)
        {
            if (!string.IsNullOrWhiteSpace(specializationName))
                return Ok(await specializationService.SearchSpecialization(specializationName));

            return Ok(await specializationService.GetAllSpecializations());
        }
    }
}
