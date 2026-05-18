using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.PracticeMaterials
{
    [Authorize(Roles = Roles.Employer)]
    [Route("api/employers/me/practice-offers/{practiceOfferId:int}/files")]
    public class EmployerPracticeMaterialsController(IPracticeMaterialService practiceMaterialService) : AuthorizedUserController
    {
        [HttpPost]
        public async Task<IActionResult> UploadMaterials([FromRoute] int practiceOfferId, IEnumerable<IFormFile> files)
        {
            await practiceMaterialService.UploadMaterials(UserId, practiceOfferId, files);

            return Ok();
        }

        [HttpDelete("{fileId:int}")]
        public async Task<IActionResult> DeleteFile([FromRoute] int practiceOfferId, [FromRoute] int fileId)
        {
            await practiceMaterialService.DeleteFile(UserId, practiceOfferId, fileId);

            return Ok();
        }
    }
}
