using CyberQuiz.DAL.Models;
using CyberQuiz.DAL.Repositories;
using CyberQuiz.Shared.DTOs;

namespace CyberQuiz.BLL.Services
{
    public class QuizService : IQuizService
    {
        private readonly QuestionRepository _questionRepository;

        public QuizService(QuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        // TODO: Implementera när CategoryRepository är klar i DAL.
        // 1. Hämta alla kategorier med subkategorier och frågor inladdade
        //    via CategoryRepository.GetAllCategoriesWithSubCategoriesAndQuestionsAsync()
        // 2. Mappa till CategoryDto — sätt QuestionCount från antalet frågor per subkategori
        // 3. Sätt IsUnlocked per subkategori genom att anropa IsSubCategoryUnlockedAsync(userId, sc.Id)
        //    (håller logiken på ett ställe istället för att duplicera 80%-beräkningen här)
        public Task<List<CategoryDto>> GetAllCategoriesAsync(string userId)
        {
            throw new NotImplementedException(
                "Väntar på CategoryRepository i DAL. " +
                "Behöver: GetAllCategoriesWithSubCategoriesAndQuestionsAsync()");
        }

        // Hämtar alla frågor för en given subkategori, inklusive svarsalternativ.
        // IsCorrect skickas INTE med i AnswerOptionDto — det är ett medvetet säkerhetsbeslut
        // så att rätt svar inte avslöjas i webbläsarens DevTools.
        //
        // OBS: QuestionRepository.GetQuestionBySubcategory() laddar just nu INTE in AnswerOptions
        // (saknar Include). Den som sitter med DAL behöver uppdatera den metoden
        // med .Include(q => q.AnswerOptions) för att svarsalternativen ska följa med.
        public Task<List<QuestionDto>> GetQuestionsBySubCategoryAsync(int subCategoryId)
        {
            // Hämtar frågor för subkategorin via repository
            // GetQuestionBySubcategory är synkron i DAL — wrappas i Task.FromResult
            var questions = _questionRepository
                .GetQuestionBySubcategory(subCategoryId);

            // Omvandlar varje Question-modell till QuestionDto
            var result = questions.Select(q => new QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
                SubCategoryId = q.SubCategoryId,

                // AnswerOptions kan vara tom lista tills DAL-metoden uppdateras med Include
                AnswerOptions = q.AnswerOptions?.Select(a => new AnswerOptionDto
                {
                    Id = a.Id,
                    Text = a.Text
                    // IsCorrect utelämnas medvetet — säkerhetskrav
                }).ToList() ?? []
            }).ToList();

            return Task.FromResult(result);
        }

        // TODO: Implementera när UserResultRepository och CategoryRepository är klara i DAL.
        // 1. Hämta föregående subkategori (den med närmast lägre OrderIndex inom samma kategori)
        //    via CategoryRepository.GetSubCategoryWithSiblingsAsync(subCategoryId)
        // 2. Hämta användarens svar för den föregående subkategorin
        //    via UserResultRepository.GetResultsByUserAndSubCategoryAsync(userId, föregåendeSubCategoryId)
        // 3. Räkna ut andelen rätta svar — om >= 80% returnera true, annars false
        // 4. Första subkategorin i en kategori är alltid upplåst (ingen föregående att klara)
        // OBS: Progressionen är per användare — lagras inte i DAL utan beräknas här i BLL varje gång
        public Task<bool> IsSubCategoryUnlockedAsync(string userId, int subCategoryId)
        {
            throw new NotImplementedException(
                "Väntar på UserResultRepository och CategoryRepository i DAL. " +
                "Behöver: GetUserResultsBySubCategoryAsync(userId, subCategoryId) " +
                "och GetSubCategoryWithSiblingsAsync(subCategoryId)");
        }
    }
}
