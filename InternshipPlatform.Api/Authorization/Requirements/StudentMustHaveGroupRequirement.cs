using Microsoft.AspNetCore.Authorization;

namespace InternshipPlatform.Api.Authorization.Requirements
{
    public sealed class StudentMustHaveGroupRequirement : IAuthorizationRequirement
    {
    }
}
