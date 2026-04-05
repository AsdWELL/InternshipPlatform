using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InternshipPlatform.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class AuthorizedUserController : ControllerBase
    {
        public int UserId => Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public string Email => User.FindFirstValue(ClaimTypes.Email)!;

        public string Role => User.FindFirstValue(ClaimTypes.Role)!;

        [NonAction]
        protected void ClearToken(string cookieTitle)
        {
            Response.Cookies.Delete(cookieTitle);
        }
    }
}