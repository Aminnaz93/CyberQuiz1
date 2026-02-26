using CyberQuiz.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CyberQuiz.DAL.Repositories
{
    // Repository för databasoperationer mot Questions-tabellen
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext _context;

        // Dependency injection av DbContext
        public QuestionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Question>> GetAllQuestionsAsync()
        {
            return await _context.Questions.ToListAsync();
        }

        public async Task<Question?> GetQuestionByIdAsync(int id)
        {
            return await _context.Questions.FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<IEnumerable<Question>> GetQuestionBySubcategoryAsync(int subCategory)
        {
            return await _context.Questions.Where(q => q.SubCategoryId == subCategory).ToListAsync();
        }

        //Tveksamt om vi kommer att uppdatera frågebanken, men det skadar ju aldrig!
        public async Task AddQuestionAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuestionAsync(Question question)
        {
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuestionAsync(int id)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == id);
            if (question != null)
            {
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }
        }
    }
}
