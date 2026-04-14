using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.Resume;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.Resumes
{
    [Authorize(Roles = Roles.Student)]
    [Route("api/students/me/resumes/{resumeId:int}/work-experiences")]
    public class ResumeWorkExperiencesController(IResumeService resumeService) : AuthorizedUserController
    {
        [HttpPost]
        public async Task<IActionResult> AddWorkExperience([FromRoute] int resumeId, [FromBody] AddWorkExperienceRequest request)
        {
            request.StudentId = UserId;
            request.ResumeId = resumeId;

            return Ok(await resumeService.AddWorkExperience(request));
        }

        [HttpPut("{workExperienceId:int}")]
        public async Task<IActionResult> UpdateWorkExperience(
            [FromRoute] int resumeId,
            [FromRoute] int workExperienceId,
            [FromBody] UpdateWorkExperienceRequest request)
        {
            request.StudentId = UserId;
            request.Id = workExperienceId;
            request.ResumeId = resumeId;

            await resumeService.UpdateWorkExperience(request);

            return Ok();
        }

        [HttpDelete("{workExperienceId:int}")]
        public async Task<IActionResult> DeleteWorkExperience([FromRoute] int workExperienceId)
        {
            await resumeService.DeleteWorkExperience(workExperienceId);

            return Ok();
        }
    }
}
