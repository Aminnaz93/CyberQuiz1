using CyberQuiz.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberQuiz.DAL.Repositories
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        //DependencyInjection av DbContext
        private readonly ApplicationDbContext _context;

        public SubCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(int categoryId)
        {
            return await _context.SubCategories
                .Where(sc => sc.CategoryId == categoryId)
                .OrderBy(sc => sc.OrderIndex)  // För att förhindra att ordningen ändras okontrollerat
                .ToListAsync();
        }

        public async Task<IEnumerable<SubCategory>> GetAllSubCategoriesAsync()
        {
            return await _context.SubCategories
                .OrderBy(sc => sc.CategoryId).ThenBy(sc => sc.OrderIndex)  // För att förhindra att ordningen ändras okontrollerat
                .ToListAsync();
        }

        public async Task<SubCategory?> GetSubCategoryByIdAsync(int id)
        {
            return await _context.SubCategories.FirstOrDefaultAsync(sc => sc.Id == id);
        }

        public async Task<IEnumerable<SubCategory>> GetAllSubCategoriesWithQuestionsAsync()
        {
            return await _context.SubCategories
                .Include(sc => sc.Questions)
                .ToListAsync();
        }
        public async Task<SubCategory?> GetSubCategoryWithQuestionsAsync(int id)
        {
            return await _context.SubCategories
                .Include(sc => sc.Questions)
                .FirstOrDefaultAsync(sc => sc.Id == id);
        }


    }
}
