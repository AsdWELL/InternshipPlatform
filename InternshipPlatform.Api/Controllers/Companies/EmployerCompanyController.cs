using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.Company;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.Companies
{
    [Authorize(Roles = Roles.Employer)]
    [Route("api/employers/me/company")]
    public class EmployerCompanyController(ICompanyService companyService) : AuthorizedUserController
    {
        [HttpGet]
        public async Task<IActionResult> GetEmployerCompany()
        {
            return Ok(await companyService.GetEmployerCompany(UserId));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyRequest request)
        {
            request.EmployerId = UserId;

            await companyService.UpdateCompany(request);

            return Ok();
        }

        [HttpPut("logo")]
        public async Task<IActionResult> UpdateCompanyLogoByEmployerId(IFormFile logoFile)
        {
            await companyService.UpdateCompanyLogoByEmployerId(UserId, logoFile);

            return Ok();
        }

        [HttpDelete("logo")]
        public async Task<IActionResult> DeleteCompanyLogoByEmployerId()
        {
            await companyService.DeleteCompanyLogoByEmployerId(UserId);

            return Ok();
        }
    }
}
