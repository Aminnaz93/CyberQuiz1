namespace CyberQuiz.Shared.DTOs
{
    // Skickas från UI när användare vill ha studieråd
    public class AIRecommendationRequestDto
    {
        // Användarens ID
        public string UserId { get; set; } = string.Empty;

        // Filtrera på specifik subkategori (null = alla)
        public int? SubCategoryId { get; set; }
    }
}
