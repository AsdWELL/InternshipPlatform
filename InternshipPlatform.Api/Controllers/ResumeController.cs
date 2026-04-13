using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.Resume;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    public class ResumeController(IResumeService resumeService) : AuthorizedUserController
    {
        [Authorize(Roles = Roles.Student)]
        [HttpPost]
        public async Task<IActionResult> CreateResume([FromBody] CreateResumeRequest request)
        {
            request.StudentId = UserId;

            return Ok(await resumeService.CreateResume(request));
        }

        [Authorize(Roles = Roles.Student)]
        [HttpGet("my-resumes")]
        public async Task<IActionResult> GetStudentResumes()
        {
            return Ok(await resumeService.GetStudentResumes(UserId));
        }

        [HttpGet("{resumeId:int}")]
        public async Task<IActionResult> GetResumeDetails([FromRoute] int resumeId)
        {
            return Ok(await resumeService.GetResumeDetails(UserId, resumeId));
        }

        [Authorize(Roles = Roles.Employer)]
        [HttpGet("recommended")]
        public async Task<IActionResult> GetRecommendedResumes([FromQuery] PageRequestParameters pageParameters)
        {
            var request = new GetRecommendedResumesRequest
            {
                EmployerId = UserId,
                PageIndex = pageParameters.PageIndex,
                PageSize = pageParameters.PageSize
            };

            return Ok(await resumeService.GetRecommendedResumes(request));
        }

        [Authorize(Roles = Roles.Employer)]
        [HttpGet("recommended/by-vacancy/{vacancyId:int}")]
        public async Task<IActionResult> GetRecommendedResumesForVacancy([FromRoute] int vacancyId, [FromQuery] PageRequestParameters pageParameters)
        {
            var request = new GetRecommendedResumesForVacancyRequest
            {
                EmployerId = UserId,
                VacancyId = vacancyId,
                PageIndex = pageParameters.PageIndex,
                PageSize = pageParameters.PageSize
            };

            return Ok(await resumeService.GetRecommendedResumesForVacancy(request));
        }

        [Authorize(Roles = Roles.Employer)]
        [HttpGet("search")]
        public async Task<IActionResult> SearchResumes([FromQuery] SearchResumeParameters parameters)
        {
            return Ok(await resumeService.SearchResumes(parameters));
        }

        [Authorize(Roles = Roles.Student)]
        [HttpPut]
        public async Task<IActionResult> UpdateResume([FromBody] UpdateResumeRequest request)
        {
            request.StudentId = UserId;

            await resumeService.UpdateResume(request);

            return Ok();
        }

        [Authorize(Roles = Roles.Student)]
        [HttpPost("work-experience")]
        public async Task<IActionResult> AddWorkExperience([FromBody] AddWorkExperienceRequest request)
        {
            request.StudentId = UserId;

            return Ok(await resumeService.AddWorkExperience(request));
        }

        [Authorize(Roles = Roles.Student)]
        [HttpPut("work-experience")]
        public async Task<IActionResult> UpdateWorkExperience([FromBody] UpdateWorkExperienceRequest request)
        {
            request.StudentId = UserId;

            await resumeService.UpdateWorkExperience(request);

            return Ok();
        }

        [Authorize(Roles = Roles.Student)]
        [HttpDelete("work-experience/{workExperienceId:int}")]
        public async Task<IActionResult> DeleteWorkExperience([FromRoute] int workExperienceId)
        {
            await resumeService.DeleteWorkExperience(workExperienceId);

            return Ok();
        }

        [Authorize(Roles = Roles.Student)]
        [HttpDelete("{resumeId:int}")]
        public async Task<IActionResult> DeleteResume([FromRoute] int resumeId)
        {
            await resumeService.DeleteResume(UserId, resumeId);

            return Ok();
        }

        [Authorize(Roles = Roles.Student)]
        [HttpPost("copy")]
        public async Task<IActionResult> CopyResume([FromBody] int resumeId)
        {
            await resumeService.CopyResume(UserId, resumeId);

            return Ok();
        }
    }
}
