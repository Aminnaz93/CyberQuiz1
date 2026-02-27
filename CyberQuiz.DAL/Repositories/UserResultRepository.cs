using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyberQuiz.DAL.Models;
using Microsoft.EntityFrameworkCore;
namespace CyberQuiz.DAL.Repositories
{
    public class UserResultRepository : IUserResultRepository
    {
        private readonly ApplicationDbContext _context;

        // Dependency injection av DbContext
        public UserResultRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        // alla UserResult inklusive fråga och svarsalternativ
        public async Task<IEnumerable<UserResult>> GetAllUserResultsByUserIdAsync(string userId)
        {
            return await _context.UserResults
                .Include(ur => ur.Question)
                .Include(ur => ur.AnswerOption)
                .Where(ur => ur.UserId == userId)
                .OrderByDescending(ur => ur.AnsweredAt)  // Senaste först
                .ToListAsync();
        }
        // för att kolla om användaren svarat på en fråga och vad, i så fall
        public async Task<UserResult?> GetUserResultByUserAndQuestionAsync(string userId, int questionId)
        {
            return await _context.UserResults
                .Include(ur => ur.AnswerOption)  // Se vad användaren svarade
                .Where(ur => ur.UserId == userId && ur.QuestionId == questionId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserResult>> GetAllUserResultsAsync()
        {
            return await _context.UserResults.ToListAsync();

        }

        public async Task<UserResult?> GetUserResultByIdAsync(int id)
        {
            return await _context.UserResults.FirstOrDefaultAsync(ur => ur.Id == id);
        }

        public async Task<IEnumerable<UserResult>> GetUserProgressByUserAndSubcategoryAsync(string userId, int subCategoryId)
        {
            return await _context.UserResults
                .Include(ur => ur.Question)
                .Where(ur => ur.UserId == userId && ur.Question.SubCategoryId == subCategoryId)
                .OrderBy(ur => ur.AnsweredAt)
                .ToListAsync();
        }
        //Spara ett svar
        public async Task AddUserResultAsync(UserResult userResult)
        {
            _context.UserResults.Add(userResult);
            await _context.SaveChangesAsync();
        }

    }
}