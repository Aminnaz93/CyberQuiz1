using CyberQuiz.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberQuiz.DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        //DependencyInjection av DbContext
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .OrderBy(c => c.Id)  // Alltid samma ordning
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesWithSubCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.SubCategories)
                .OrderBy(c => c.Id)  // Alltid samma ordning
                .ToListAsync();
        }
        public async Task<Category?> GetCategoryWithSubCategoriesAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
