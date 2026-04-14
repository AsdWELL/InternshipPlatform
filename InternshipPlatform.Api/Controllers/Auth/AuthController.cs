using InternshipPlatform.Application.Dtos.Auth;
using InternshipPlatform.Application.Dtos.User;
using InternshipPlatform.Application.Interfaces.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InternshipPlatform.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(
        IAuthService authService,
        IOptions<TokenOptions> tokenOptions) : ControllerBase
    {
        public TokenOptions TokenOptionsValue => tokenOptions.Value;

        [NonAction]
        private void SetTokenInCookie(string accessToken)
        {
            Response.Cookies.Append(TokenOptionsValue.CookieTitle, accessToken);
        }

        [HttpPost("register/student")]
        public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentRequest request)
        {
            var authResponse = await authService.RegisterStudent(request);

            SetTokenInCookie(authResponse.AccessToken);

            return Ok(authResponse);
        }

        [HttpPost("register/employer")]
        public async Task<IActionResult> RegisterEmployer([FromBody] RegisterCompanyRequest request)
        {
            var authResponse = await authService.RegisterEmployer(request);

            SetTokenInCookie(authResponse.AccessToken);

            return Ok(authResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var authResponse = await authService.Login(request);

            SetTokenInCookie(authResponse.AccessToken);

            return Ok(authResponse);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var authResponse = await authService.RefreshToken(refreshToken);

            SetTokenInCookie(authResponse.AccessToken);

            return Ok(authResponse);
        }
    }
}
