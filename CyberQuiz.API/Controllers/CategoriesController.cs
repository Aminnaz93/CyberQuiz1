// Hämtar IQuizService från BLL/Services-mappen
using CyberQuiz.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberQuiz.API.Controllers
{
    // Markerar att detta är en API-controller
    [ApiController]
    // API-adressen
    [Route("api/[controller]")]
    // Bara inloggade användare får använda denna
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        // Håller en referens till BLL
        private readonly IQuizService _quizService;

        // Tar emot IQuizService automatiskt via dependency injection
        public CategoriesController(IQuizService quizService)
        {
            _quizService = quizService;
        }
        // Svarar på GET-anrop till /api/categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            // Hämtar den inloggade användarens id från sessionen
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Om userId saknas är inte användren inloggad
            if (userId == null)
                return Unauthorized();
            // tar alla kategorier från BLL med användarens id
            var categories = await _quizService.GetAllCategoriesAsync(userId);
            // Skickar tillbaka kategorierna som JSON till UI
            return Ok(categories);
        }
    }
}