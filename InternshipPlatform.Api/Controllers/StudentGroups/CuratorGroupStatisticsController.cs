using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.StudentGroup;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.StudentGroups
{
    [Authorize(Roles = Roles.Teacher)]
    [Route("api/curators/me/groups/{groupId:int}/students")]
    public class CuratorGroupStatisticsController(ICuratorGroupStatisticsService statisticsService) : AuthorizedUserController
    {
        [HttpGet("{studentId:int}")]
        public async Task<IActionResult> GetStudentDetails([FromRoute] int groupId, [FromRoute] int studentId)
        {
            var request = new CuratorGroupStatisticsRequest
            {
                CuratorId = UserId,
                StudentId = studentId,
                GroupId = groupId
            };

            return Ok(await statisticsService.GetStudentDetails(request));
        }

        [HttpGet("{studentId:int}/resumes")]
        public async Task<IActionResult> GetStudentResumes([FromRoute] int groupId, [FromRoute] int studentId)
        {
            var request = new CuratorGroupStatisticsRequest
            {
                CuratorId = UserId,
                StudentId = studentId,
                GroupId = groupId
            };

            return Ok(await statisticsService.GetStudentResumes(request));
        }

        [HttpGet("{studentId:int}/application-stats")]
        public async Task<IActionResult> GetApplicationsStatistics([FromRoute] int groupId, [FromRoute] int studentId)
        {
            var request = new CuratorGroupStatisticsRequest
            {
                CuratorId = UserId,
                StudentId = studentId,
                GroupId = groupId
            };

            return Ok(await statisticsService.GetApplicationsStatistics(request));
        }
    }
}
