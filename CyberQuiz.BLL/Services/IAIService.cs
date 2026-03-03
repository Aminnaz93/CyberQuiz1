using CyberQuiz.Shared.DTOs;

namespace CyberQuiz.BLL.Services
{
    public interface IAIService
    {
        // Analyserar användarens quiz-resultat och genererar AI-baserade studieråd
        Task<AIRecommendationResponseDto> GetStudyRecommendationsAsync(string userId, int? subCategoryId = null);
        
        // Testar Ollama-anslutningen
        Task<bool> HealthCheckAsync();
    }
}
