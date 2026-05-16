using InternshipPlatform.Api.Authorization;
using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.PracticeOffer;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.PracticeOffers
{
    [Authorize(Policy = Policies.ApprovedUser)]
    public class PracticeOffersController(IPracticeOfferService practiceOfferService) : AuthorizedUserController
    {
        [HttpGet("api/practice-offers/{practiceOfferId:int}")]
        public async Task<IActionResult> GetPracticeOfferDetails([FromRoute] int practiceOfferId)
        {
            return Role switch
            {
                Roles.Student => Ok(await practiceOfferService.GetPracticeOfferDetailsForStudent(practiceOfferId)),
                Roles.Employer => Ok(await practiceOfferService.GetPracticeOfferDetailsForOwner(UserId, practiceOfferId)),
                _ => Forbid()
            };
        }

        [HttpGet("api/practice-offers")]
        public async Task<IActionResult> SearchPracticeOffers([FromQuery] SearchPracticeOfferParameters parameters)
        {
            parameters.StudentId = UserId;

            return Ok(await practiceOfferService.SearchPracticeOffers(parameters));
        }

        [HttpGet("api/companies/{companyId:int}/practice-offers")]
        public async Task<IActionResult> GetCompanyPracticeOffers([FromRoute] int companyId)
        {
            return Ok(await practiceOfferService.GetCompanyPracticeOffers(companyId));
        }
    }
}
