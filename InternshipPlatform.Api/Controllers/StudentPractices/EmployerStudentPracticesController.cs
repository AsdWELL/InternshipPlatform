using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.StudentPractices
{
    [Authorize(Roles = Roles.Employer)]
    [Route("api/employers/me/student-practices")]
    public class EmployerStudentPracticesController(IStudentPracticeService studentPracticeService) : AuthorizedUserController
    {
        [HttpGet]
        public async Task<IActionResult> GetStudentPractices()
        {
            return Ok(await studentPracticeService.GetEmployerStudentPractices(UserId));
        }

        [HttpGet("{studentPracticeId:int}")]
        public async Task<IActionResult> GetStudentPracticeDetails([FromRoute] int studentPracticeId)
        {
            return Ok(await studentPracticeService.GetEmployerStudentPracticeDetails(UserId, studentPracticeId));
        }

        [HttpGet("{studentPracticeId:int}/report")]
        public async Task<IActionResult> DownloadReport([FromRoute] int studentPracticeId)
        {
            var file = await studentPracticeService.DownloadEmployerStudentReport(UserId, studentPracticeId);

            return File(file.Stream, file.ContentType, file.FileName);
        }

        [HttpGet("{studentPracticeId:int}/solution")]
        public async Task<IActionResult> DownloadSolution([FromRoute] int studentPracticeId)
        {
            var file = await studentPracticeService.DownloadEmployerStudentSolution(UserId, studentPracticeId);

            return File(file.Stream, file.ContentType, file.FileName);
        }

        [HttpPut("{studentPracticeId:int}/submission/accept")]
        public async Task<IActionResult> AcceptSubmission([FromRoute] int studentPracticeId)
        {
            await studentPracticeService.AcceptStudentPracticeSubmission(UserId, studentPracticeId);

            return Ok();
        }

        [HttpPut("{studentPracticeId:int}/submission/revision")]
        public async Task<IActionResult> SendSubmissionToRevision([FromRoute] int studentPracticeId)
        {
            await studentPracticeService.SendStudentPracticeSubmissionToRevision(UserId, studentPracticeId);

            return Ok();
        }
    }
}
