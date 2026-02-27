using CyberQuiz.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberQuiz.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubCategoriesController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public SubCategoriesController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        // Svarar på GET /api/subcategories/{categoryId}
        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetSubCategories(int categoryId)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            // Hämtar alla kategorier och plockar ut rätt en baserat på categoryId
            var categories = await _quizService.GetAllCategoriesAsync(userId);
            var category = categories.FirstOrDefault(c => c.Id == categoryId);

            if (category == null)
                return NotFound();

            // Skickar tillbaka subkategorierna med IsUnlocked satt av BLL
            return Ok(category.SubCategories);
        }
    }
}