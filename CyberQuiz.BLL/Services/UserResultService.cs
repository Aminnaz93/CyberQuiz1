using CyberQuiz.Shared.DTOs;

namespace CyberQuiz.BLL.Services
{
    public class UserResultService : IUserResultService
    {
        // Inga repositories injiceras ännu — UserResultRepository är ej klar i DAL.
        // Lägg till dem här i konstruktorn när de finns:
        // private readonly UserResultRepository _userResultRepository;
        // private readonly QuestionRepository _questionRepository; (behövs för att kolla rätt svar)

        // TODO: Implementera när UserResultRepository och QuestionRepository är klara i DAL.
        // 1. Hämta AnswerOption för AnswerSubmitDto.SelectedAnswerOptionId
        //    via QuestionRepository (eller AnswerOptionRepository om sådan skapas)
        //    — behövs för att kontrollera om IsCorrect är true samt vilket svar som var rätt
        // 2. Skapa ett nytt UserResult-objekt med:
        //    - UserId från AnswerSubmitDto.UserId
        //    - QuestionId från AnswerSubmitDto.QuestionId
        //    - AnswerOptionId från AnswerSubmitDto.SelectedAnswerOptionId
        //    - IsCorrect från det hämtade svarsalternativets IsCorrect-flagga
        //    - AnsweredAt = DateTimeOffset.UtcNow
        // 3. Spara UserResult via UserResultRepository.AddAsync(userResult)
        // 4. Returnera AnswerResultDto med IsCorrect och CorrectAnswerOptionId
        public Task<AnswerResultDto> SubmitAnswerAsync(AnswerSubmitDto answerSubmit)
        {
            throw new NotImplementedException(
                "Väntar på UserResultRepository i DAL. " +
                "Behöver även tillgång till AnswerOption för att kontrollera rätt svar.");
        }

        // TODO: Implementera när UserResultRepository är klar i DAL.
        // 1. Hämta alla UserResults för användaren
        //    via UserResultRepository.GetResultsByUserAsync(userId)
        // 2. Gruppera resultaten per subkategori (via Question.SubCategoryId)
        // 3. För varje subkategori: räkna ut om användaren nått >= 80% rätt (IsPassed)
        // 4. Returnera en lista av ProgressionDto med UserId, SubCategoryId och IsPassed
        public Task<List<ProgressionDto>> GetProgressionByUserAsync(string userId)
        {
            throw new NotImplementedException(
                "Väntar på UserResultRepository i DAL. " +
                "Behöver: GetResultsByUserAsync(userId)");
        }
    }
}
