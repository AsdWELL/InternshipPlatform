using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.StudentGroup;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.StudentGroups
{
    [Authorize(Roles = Roles.Curator)]
    [Route("api/curators/me/groups")]
    public class CuratorGroupsController(IStudentGroupService studentGroupService) : AuthorizedUserController
    {
        [HttpPost]
        public async Task<IActionResult> CreateStudentGroup([FromBody] CreateStudentGroupRequest request)
        {
            request.CuratorId = UserId;

            return Ok(await studentGroupService.CreateStudentGroup(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetMyGroups()
        {
            return Ok(await studentGroupService.GetCuratorGroups(UserId));
        }

        [HttpGet("{groupId:int}")]
        public async Task<IActionResult> GetGroupDetails([FromRoute] int groupId)
        {
            return Ok(await studentGroupService.GetGroupDetailsForCurator(UserId, groupId));
        }

        [HttpPut("{groupId:int}/invite-code")]
        public async Task<IActionResult> RefreshInviteCode([FromRoute] int groupId)
        {
            return Ok(await studentGroupService.RefreshInviteCode(UserId, groupId));
        }
    }
}
