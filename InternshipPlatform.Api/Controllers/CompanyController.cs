using InternshipPlatform.Application.Dtos.Company;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    public class CompanyController(ICompanyService companyService) : AuthorizedUserController
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCompanyById([FromRoute] int id)
        {
            return Ok(await companyService.GetCompanyById(id));
        }

        [Authorize(Roles = Roles.Employer)]
        [HttpPut]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyRequest request)
        {
            request.EmployerId = UserId;

            await companyService.UpdateCompany(request);

            return Ok();
        }

        [Authorize(Roles = Roles.Employer)]
        [HttpPost("logo")]
        public async Task<IActionResult> UpdateCompanyLogoByEmployerId(IFormFile logoFile)
        {
            await companyService.UpdateCompanyLogoByEmployerId(UserId, logoFile);

            return Ok();
        }

        [Authorize(Roles = Roles.Employer)]
        [HttpDelete("logo")]
        public async Task<IActionResult> DeleteCompanyLogoByEmployerId()
        {
            await companyService.DeleteCompanyLogoByEmployerId(UserId);

            return Ok();
        }
    }
}
