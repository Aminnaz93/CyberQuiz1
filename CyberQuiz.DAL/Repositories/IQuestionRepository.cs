using CyberQuiz.DAL.Models;

namespace CyberQuiz.DAL.Repositories
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<Question>> GetAllQuestionsAsync();
        Task<Question?> GetQuestionByIdAsync(int id);
        Task<IEnumerable<Question>> GetQuestionBySubcategoryAsync(int subCategory);
        Task AddQuestionAsync(Question question);
        Task UpdateQuestionAsync(Question question);
        Task DeleteQuestionAsync(int id);
    }
}
