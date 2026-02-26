using CyberQuiz.Shared.DTOs;

namespace CyberQuiz.BLL.Services
{
    public interface IQuizService
    {
        // Hämtar alla kategorier med subkategorier och korrekt IsUnlocked per användare
        Task<List<CategoryDto>> GetAllCategoriesAsync(string userId);

        // Hämtar alla frågor för en specifik subkategori
        Task<List<QuestionDto>> GetQuestionsBySubCategoryAsync(int subCategoryId);

        // Kontrollerar om en subkategori är upplåst för en användare (80%-regeln)
        Task<bool> IsSubCategoryUnlockedAsync(string userId, int subCategoryId);
    }
}
