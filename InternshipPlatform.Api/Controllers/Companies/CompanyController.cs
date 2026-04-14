using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.Companies
{
    [Route("api/companies")]
    public class CompanyController(ICompanyService companyService) : AuthorizedUserController
    {
        [HttpGet("{companyId:int}")]
        public async Task<IActionResult> GetCompanyById([FromRoute] int companyId)
        {
            return Ok(await companyService.GetCompanyById(companyId));
        }
    }
}
