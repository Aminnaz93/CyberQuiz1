using CyberQuiz.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CyberQuiz.Shared.DTOs;

namespace CyberQuiz.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly IUserResultService _userResultService;

        public QuizController(IQuizService quizService, IUserResultService userResultService)
        {
            _quizService = quizService;
            _userResultService = userResultService;
        }

        // Svarar på GET /api/quiz/{subCategoryId} — hämtar alla frågor för en subkategori
        [HttpGet("{subCategoryId}")]
        public async Task<IActionResult> GetQuestions(int subCategoryId)
        {
            var questions = await _quizService.GetAllQuestionsBySubCategoryAsync(subCategoryId);

            if (questions == null || !questions.Any())
                return NotFound();

            return Ok(questions);
        }

        // Svarar på POST /api/quiz/answer — tar emot användarens svar och returnerar rätt/fel
        [HttpPost("answer")]
        public async Task<IActionResult> SubmitAnswer([FromBody] AnswerSubmitDto answerSubmit)
        {
            // Hämtar den inloggade användarens id och lägger in det i DTO:n
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            // Sätter userId på DTO:n innan den skickas vidare till BLL
            var submitWithUser = new AnswerSubmitDto
            {
                QuestionId = answerSubmit.QuestionId,
                SelectedAnswerOptionId = answerSubmit.SelectedAnswerOptionId,
                UserId = userId
            };

            // BLL räknar ut rätt/fel och sparar resultatet i databasen
            var result = await _userResultService.SubmitAnswerAsync(submitWithUser);

            return Ok(result);
        }
    }
}