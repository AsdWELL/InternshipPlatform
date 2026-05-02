using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.Curators
{
    [Route("api/curators")]
    public class CuratorProfilesController(ICuratorService curatorService) : AuthorizedUserController
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCuratorById([FromRoute] int id)
        {
            return Ok(await curatorService.GetCuratorById(id));
        }
    }
}
