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
        private readonly ILogger<AIController> _logger;

        public AIController(IAIService aiService, ILogger<AIController> logger)
        {
            _aiService = aiService;
            _logger = logger;
        }

        // Hämta AI:ns studieråd baserat på quiz-resultaten
        [HttpGet("recommendations")]
        // [Authorize]  // TEMPORÄRT AVAKTIVERAD FÖR TESTNING
        public async Task<ActionResult<AIRecommendationResponseDto>> GetRecommendations(
            [FromQuery] int? subCategoryId = null,
            [FromQuery] string? testUserId = null)  // TEMPORÄR TEST-PARAMETER
        {
            // TEMPORÄR KOD: Använd testUserId om angiven, annars hämta från claims
            var userId = testUserId ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "Ange testUserId parameter, t.ex. ?testUserId=din-user-id" });
            }

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

        // Testa Ollama-anslutningen
        [HttpGet("health")]
        public async Task<IActionResult> HealthCheck()
        {
            var isHealthy = await _aiService.HealthCheckAsync();
            
            if (isHealthy)
            {
                return Ok(new { status = "healthy", message = "Ollama is running" });
            }
            else
            {
                return StatusCode(503, new { status = "unhealthy", message = "Cannot connect to Ollama" });
            }
        }
    }
}
