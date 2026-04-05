using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    public class SpecializationController(ISpecializationService specializationService) : AuthorizedUserController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllSpecializations()
        {
            return Ok(await specializationService.GetAllSpecializations());
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchSpecialization([FromQuery] string specializationName)
        {
            return Ok(await specializationService.SearchSpecialization(specializationName));
        }
    }
}
