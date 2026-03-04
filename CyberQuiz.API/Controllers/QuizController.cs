using CyberQuiz.BLL.Services;
using CyberQuiz.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{subCategoryId}")]
        public async Task<IActionResult> GetQuestions(int subCategoryId)
        {
            var questions = await _quizService.GetAllQuestionsBySubCategoryAsync(subCategoryId);

            if (questions == null || !questions.Any())
                return NotFound();

            return Ok(questions);
        }

        [HttpPost("answer")]
        public async Task<IActionResult> SubmitAnswer([FromBody] AnswerSubmitDto answerSubmit)
        {
            if (string.IsNullOrEmpty(answerSubmit.UserId))
                return BadRequest("userId krävs");

            var result = await _userResultService.SubmitAnswerAsync(answerSubmit);
            return Ok(result);
        }
    }
}