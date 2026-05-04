using InternshipPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers
{
    [Route("api/universities")]
    public class UniversitiesController(IUniversityService universityService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> SearchUniversities([FromQuery] string? universityName)
        {
            if (!string.IsNullOrEmpty(universityName))
                return Ok(await universityService.SearchUniversities(universityName));

            return Ok(await universityService.GetAllUniversities());
        }
    }
}
