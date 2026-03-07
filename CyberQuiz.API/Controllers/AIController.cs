using CyberQuiz.BLL.Services;
using CyberQuiz.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CyberQuiz.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private readonly IAIService _aiService;
        //Hantera chatten
        private readonly IAIService2 _aiService2;
        private readonly ILogger<AIController> _logger;

        //IAIService2 tillagd så controllern kan använda chat - tjänsten
        public AIController(IAIService aiService, IAIService2 aiService2, ILogger<AIController> logger)
        {
            _aiService = aiService;
            _aiService2 = aiService2;
            _logger = logger;
        }

        [HttpGet("recommendations")]
        public async Task<ActionResult<AIRecommendationResponseDto>> GetRecommendations(
            [FromQuery] int? subCategoryId = null,
            [FromQuery] string? testUserId = null)
        {
            var userId = testUserId ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return BadRequest(new { message = "Ange testUserId parameter" });

            try
            {
                var recommendations = await _aiService.GetStudyRecommendationsAsync(userId, subCategoryId);
                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate AI recommendations for user {UserId}", userId);
                return StatusCode(500, new { message = "Kunde inte generera rekommendationer. Kontrollera att Ollama körs." });
            }
        }

        // NYTT: tar emot meddelande från Coach.razor och skickar till AIService2
        // AIChatRequestDto innehåller UserId + Message
        // AIChatResponseDto innehåller AI:ns svar tillbaka
        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] AIChatRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
                return BadRequest(new { message = "userId krävs." });

            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest(new { message = "Meddelandet får inte vara tomt." });

            try
            {
                var response = await _aiService2.AskAsync(request.UserId, request.Message);
                return Ok(new AIChatResponseDto { Message = response });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get AI chat response for user {UserId}", request.UserId);
                return StatusCode(500, new { message = "Kunde inte få svar från AI. Kontrollera att Ollama körs." });
            }
        }

        [HttpGet("health")]
        public async Task<IActionResult> HealthCheck()
        {
            var isHealthy = await _aiService.HealthCheckAsync();

            if (isHealthy)
                return Ok(new { status = "healthy", message = "Ollama is running" });
            else
                return StatusCode(503, new { status = "unhealthy", message = "Cannot connect to Ollama" });
        }
    }
}