using CyberQuiz.Shared.DTOs;

namespace CyberQuiz.BLL.Services
{
    public interface IUserResultService
    {
        // Tar emot användarens svar, skickar för att spara det i databasen och returnerar om det var rätt
        // samt vilket svarsalternativ som var korrekt
        Task<AnswerResultDto> SubmitAnswerAsync(AnswerSubmitDto answerSubmit);

        // Hämtar användarens progression för alla subkategorier —
        // används på profilsidan för att visa en överblick av hur långt användaren kommit
        Task<List<ProgressionDto>> GetProgressionByUserAsync(string userId);
    }
}
