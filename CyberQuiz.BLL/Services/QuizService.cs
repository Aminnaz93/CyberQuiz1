using CyberQuiz.DAL;
using CyberQuiz.Data;
using CyberQuiz.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CyberQuiz.BLL.Services
{
    public class QuizService : IQuizService
    {
        private readonly ApplicationDbContext _context;

        public QuizService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hämtar alla kategorier med subkategorier och korrekt IsUnlocked per användare
        public async Task<List<CategoryDto>> GetAllCategoriesAsync(string userId)
        {
            var categories = await _context.Categories
                .Include(c => c.SubCategories)
                    .ThenInclude(sc => sc.Questions)
                .ToListAsync();

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                SubCategories = c.SubCategories
                    .OrderBy(sc => sc.OrderIndex)
                    .Select(sc => new SubCategoryDto
                    {
                        Id = sc.Id,
                        Name = sc.Name,
                        OrderIndex = sc.OrderIndex,
                        CategoryId = sc.CategoryId,
                        QuestionCount = sc.Questions.Count,
                        IsUnlocked = IsFirstInCategory(sc, c.SubCategories) ||
                                     IsUnlockedByPreviousAsync(userId, sc, c.SubCategories).Result
                    }).ToList()
            }).ToList();
        }

        // Hämtar frågor med svarsalternativ för en subkategori
        public async Task<List<QuestionDto>> GetQuestionsBySubCategoryAsync(int subCategoryId)
        {
            var questions = await _context.Questions
                .Include(q => q.AnswerOptions)
                .Where(q => q.SubCategoryId == subCategoryId)
                .ToListAsync();

            return questions.Select(q => new QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
                SubCategoryId = q.SubCategoryId,
                AnswerOptions = q.AnswerOptions.Select(a => new AnswerOptionDto
                {
                    Id = a.Id,
                    Text = a.Text
                    // IsCorrect utelämnas medvetet — säkerhetskrav
                }).ToList()
            }).ToList();
        }

        // Kontrollerar om en subkategori är upplåst för en användare (80%-regeln)
        public async Task<bool> IsSubCategoryUnlockedAsync(string userId, int subCategoryId)
        {
            var subCategory = await _context.SubCategories
                .Include(sc => sc.Category)
                    .ThenInclude(c => c.SubCategories)
                .FirstOrDefaultAsync(sc => sc.Id == subCategoryId);

            if (subCategory == null) return false;

            // Första subkategorin i en kategori är alltid upplåst
            if (IsFirstInCategory(subCategory, subCategory.Category.SubCategories))
                return true;

            return await IsUnlockedByPreviousAsync(userId, subCategory, subCategory.Category.SubCategories);
        }

        // --- Hjälpmetoder ---

        // Kontrollerar om subkategorin är den första (lägst OrderIndex) i sin kategori
        private bool IsFirstInCategory(DAL.Models.SubCategory subCategory, IEnumerable<DAL.Models.SubCategory> siblings)
        {
            return subCategory.OrderIndex == siblings.Min(sc => sc.OrderIndex);
        }

        // Kontrollerar om föregående subkategori är avklarad med minst 80%
        private async Task<bool> IsUnlockedByPreviousAsync(
            string userId,
            DAL.Models.SubCategory current,
            IEnumerable<DAL.Models.SubCategory> siblings)
        {
            // Hitta föregående subkategori baserat på OrderIndex
            var previous = siblings
                .Where(sc => sc.OrderIndex < current.OrderIndex)
                .OrderByDescending(sc => sc.OrderIndex)
                .FirstOrDefault();

            if (previous == null) return false;

            // Hämta användarens svar för den föregående subkategorin
            var results = await _context.UserResults
                .Include(ur => ur.Question)
                .Where(ur => ur.UserId == userId && ur.Question.SubCategoryId == previous.Id)
                .ToListAsync();

            if (!results.Any()) return false;

            // Räkna ut procent rätt
            double percentCorrect = (double)results.Count(ur => ur.IsCorrect) / results.Count * 100;
            return percentCorrect >= 80;
        }
    }
}
