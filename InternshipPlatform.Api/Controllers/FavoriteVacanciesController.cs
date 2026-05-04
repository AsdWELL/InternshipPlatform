using InternshipPlatform.Api.Authorization;
using InternshipPlatform.Api.Controllers.Auth;
using InternshipPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPlatform.Api.Controllers
{
    [Authorize(Policy = Policies.StudentMustHaveGroup)]
    [Route("api/students/me/favorite-vacancies")]
    public class FavoriteVacanciesController(IFavoriteVacancyService favoriteVacancyService) : AuthorizedUserController
    {
        [HttpPost("{vacancyId:int}")]
        public async Task<IActionResult> AddToFavorites([FromRoute] int vacancyId)
        {
            await favoriteVacancyService.AddToFavorites(UserId, vacancyId);

            return Ok();
        }

        [HttpDelete("{vacancyId:int}")]
        public async Task<IActionResult> RemoveFromFavorites([FromRoute] int vacancyId)
        {
            await favoriteVacancyService.RemoveFromFavorites(UserId, vacancyId);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetFavoriteVacancies()
        {
            return Ok(await favoriteVacancyService.GetFavoriteVacancies(UserId));
        }
    }
}
