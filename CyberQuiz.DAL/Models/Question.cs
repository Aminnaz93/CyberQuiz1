namespace CyberQuiz.DAL.Models
{
    // En quiz-fråga med tillhörande svarsalternativ
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } // Själva frågetexten

        // FK till SubCategory
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        // Navigationspropertys
        public ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>(); // Frågans 3 svarsalternativ
        public ICollection<UserResult> UserResults { get; set; } = new List<UserResult>(); // Användarnas svar på denna fråga
    }
}
