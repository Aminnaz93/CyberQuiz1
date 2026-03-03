namespace CyberQuiz.Shared.DTOs
{
    // AI:s analys och studieråd
    public class AIRecommendationResponseDto
    {
        // AI:s övergripande analys av användarens kunskapsnivå
        public string Summary { get; set; } = string.Empty;
        
        // Lista med specifika studieområden
        public List<StudyRecommendationDto> Recommendations { get; set; } = new();
        
        // Totalt antal besvarade frågor
        public int TotalQuestionsAnswered { get; set; }
        
        // Antal korrekta svar
        public int CorrectAnswers { get; set; }
        
        // Framgångsgrad i procent (0-100)
        public double SuccessRate { get; set; }
        
        // Vilka kategorier som analyserades
        public List<string> AnalyzedCategories { get; set; } = new();
        
        // Svagaste kategorin
        public string? WeakestCategory { get; set; }
        
        // Starkaste kategorin
        public string? StrongestCategory { get; set; }
        
        // När analysen gjordes
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        
        // Uppskattad studietid i timmar
        public int EstimatedStudyHours { get; set; }
    }
}
