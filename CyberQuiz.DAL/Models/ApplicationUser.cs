using Microsoft.AspNetCore.Identity;


namespace CyberQuiz.DAL.Models
{
    // Användarklass som ärver från och utökar Identity med nya egenskaper
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty; // Visningsnamn i UI

        public bool IsActive { get; set; } = true; // Aktivt konto eller ej

        //Användarens quiz-resultat
        public ICollection<UserResult> UserResults { get; set; } = new List<UserResult>();
    }
}
