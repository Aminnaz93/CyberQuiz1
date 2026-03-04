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

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetSubCategories(int categoryId, [FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("userId krävs");

            var categories = await _quizService.GetAllCategoriesAsync(userId);
            var category = categories.FirstOrDefault(c => c.Id == categoryId);

            if (category == null)
                return NotFound();

            return Ok(category.SubCategories);
        }
    }
}