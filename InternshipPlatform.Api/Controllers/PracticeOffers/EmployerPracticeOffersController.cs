using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Dtos.PracticeOffer;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers.PracticeOffers
{
    [Authorize(Roles = Roles.Employer)]
    [Route("api/employers/me/practice-offers")]
    public class EmployerPracticeOffersController(IPracticeOfferService practiceOfferService) : AuthorizedUserController
    {
        [HttpPost]
        public async Task<IActionResult> CreatePracticeOffer([FromBody] CreatePracticeOfferRequest request)
        {
            request.EmployerId = UserId;

            return Ok(await practiceOfferService.CreatePracticeOffer(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetMyPracticeOffers()
        {
            return Ok(await practiceOfferService.GetEmployerPracticeOffers(UserId));
        }

        [HttpPut("{practiceOfferId:int}")]
        public async Task<IActionResult> UpdatePracticeOffer(
            [FromRoute] int practiceOfferId,
            [FromBody] UpdatePracticeOfferRequest request)
        {
            request.EmployerId = UserId;
            request.Id = practiceOfferId;

            await practiceOfferService.UpdatePracticeOffer(request);

            return Ok();
        }

        [HttpDelete("{practiceOfferId:int}")]
        public async Task<IActionResult> DeletePracticeOffer([FromRoute] int practiceOfferId)
        {
            await practiceOfferService.DeletePracticeOffer(UserId, practiceOfferId);

            return Ok();
        }
    }
}
