using InternshipPlatform.Api.Authorization.Requirements;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace InternshipPlatform.Api.Authorization.Handlers
{
    public sealed class ApprovedUserHandler(
        IStudentProfileRepository studentProfileRepository)
        : AuthorizationHandler<ApprovedUserRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ApprovedUserRequirement requirement)
        {
            if (!context.User.Identity?.IsAuthenticated ?? true)
                return;

            if (context.User.IsInRole(Roles.Employer) ||
                context.User.IsInRole(Roles.Curator))
            {
                context.Succeed(requirement);
                return;
            }

            if (!int.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out int studentId))
                return;

            if (!await studentProfileRepository.IsStudentHasGroup(studentId))
                return;

            context.Succeed(requirement);
        }
    }
}
