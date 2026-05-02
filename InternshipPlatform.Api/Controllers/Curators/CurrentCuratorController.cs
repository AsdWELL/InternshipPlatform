using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.Auth;
using InternshipPlatform.Application.Dtos.Curator;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InternshipPlatform.Api.Controllers.Curators
{
    [Authorize(Roles = Roles.Curator)]
    [Route("api/curators/me")]
    public class CurrentCuratorController(
        ICuratorService curatorService,
        IOptions<TokenOptions> tokenOptions) : AuthorizedUserController
    {
        [HttpGet]
        public async Task<IActionResult> GetCurrentCurator()
        {
            return Ok(await curatorService.GetCuratorById(UserId));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCuratorProfile([FromBody] UpdateCuratorRequest request)
        {
            request.UserId = UserId;
            
            await curatorService.UpdateCuratorProfile(request);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCurator()
        {
            await curatorService.DeleteCurator(UserId);

            ClearToken(tokenOptions.Value.CookieTitle);

            return Ok();
        }

        [HttpPut("avatar")]
        public async Task<IActionResult> UpdateCuratorAvatar(IFormFile avatarFile)
        {
            await curatorService.UpdateCuratorAvatar(UserId, avatarFile);

            return Ok();
        }

        [HttpDelete("avatar")]
        public async Task<IActionResult> DeleteAvatar()
        {
            await curatorService.DeleteAvatar(UserId);

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await curatorService.Logout(UserId);

            ClearToken(tokenOptions.Value.CookieTitle);

            return Ok();
        }
    }
}
