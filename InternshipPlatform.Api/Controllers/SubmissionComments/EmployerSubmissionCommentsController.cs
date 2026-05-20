using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.SubmissionComment;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.SubmissionComments
{
    [Authorize(Roles = Roles.Employer)]
    [Route("api/employers/me/student-practices/{studentPracticeId:int}/submission/comments")]
    public class EmployerSubmissionCommentsController(ISubmissionCommentService commentService) : AuthorizedUserController
    {
        [HttpPost]
        public async Task<IActionResult> SendSubmissionComment(
            [FromRoute] int studentPracticeId,
            [FromBody] SendSubmissionCommentByEmployerRequest request)
        {
            request.StudentPracticeId = studentPracticeId;
            request.EmployerId = UserId;
            
            await commentService.SendSubmissionCommentByEmployer(request);

            return Ok();
        }
    }
}
