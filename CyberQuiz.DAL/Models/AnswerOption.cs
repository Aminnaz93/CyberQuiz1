namespace CyberQuiz.DAL.Models
{
    // Ett svarsalternativ på en fråga (3 st per fråga, varav 1 korrekt)
    public class AnswerOption
    {
        public int Id { get; set; }
        public string Text { get; set; } // Svarstexten
        public bool IsCorrect { get; set; } // Markerar om detta är rätt svar

        // FK till Question
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        // Navigationsproperty: Användare som valt detta svar
        public ICollection<UserResult> UserResults { get; set; } = new List<UserResult>();
    }
}
