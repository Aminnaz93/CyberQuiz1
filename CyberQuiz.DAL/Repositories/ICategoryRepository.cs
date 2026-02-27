using CyberQuiz.DAL.Models;

namespace CyberQuiz.DAL.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        // Lägg till i interface:
        Task<IEnumerable<Category>> GetAllCategoriesWithSubCategoriesAsync();
        Task<Category?> GetCategoryWithSubCategoriesAsync(int id);
    }
}
