namespace CyberQuiz.Shared.DTOs
{
    // En specifik studieområde-rekommendation
    public class StudyRecommendationDto
    {
        // Ämnesområde (t.ex. "SQL Injection")
        public string Topic { get; set; } = string.Empty;
        
        // Varför användaren bör studera detta
        public string Reason { get; set; } = string.Empty;
        
        // Rekommenderade webbresurser
        public List<ResourceDto> RecommendedResources { get; set; } = new();
        
        // Antal frågor i detta ämne
        public int QuestionsInTopic { get; set; }
        
        // Antal korrekta svar i detta ämne
        public int CorrectAnswers { get; set; }
        
        // Framgångsgrad för ämnet (0-100)
        public double TopicSuccessRate { get; set; }
        
        // Prioritet (1 = viktigast)
        public int Priority { get; set; }
        
        // Svårighetsgrad (beginner/intermediate/advanced)
        public string DifficultyLevel { get; set; } = "intermediate";
        
        // Uppskattad studietid i timmar
        public int EstimatedStudyTimeHours { get; set; }
        
        // Specifika koncept att fokusera på
        public List<string> KeyConceptsToFocus { get; set; } = new();
    }
    
    // Representerar en studieresurs
    public class ResourceDto
    {
        // Titel på resursen
        public string Title { get; set; } = string.Empty;
        
        // URL till resursen
        public string Url { get; set; } = string.Empty;
        
        // Beskrivning av resursen
        public string Description { get; set; } = string.Empty;
        
        // Typ av resurs (article/video/course/documentation)
        public string Type { get; set; } = "article";
    }
}
