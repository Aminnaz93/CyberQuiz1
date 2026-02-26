using CyberQuiz.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.DAL.Repositories
{
    public class QuestionRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Question> GetAllQuestions()
        {
            return _context.Questions.ToList();
        }

        public Question GetQuestionById(int id)
        {
            return _context.Questions.FirstOrDefault(q => q.Id == id);
        }

        public Question GetQuestionBySubcategory(int subCategory)
        {
            return _context.Questions.FirstOrDefault(q => q.SubCategoryId == subCategory);
        }


        public void AddQuestion(Question question)
        {
            _context.Questions.Add(question);
            _context.SaveChanges();
        }

        public void UpdateQuestion(Question question)
        {
            _context.Questions.Update(question);
            _context.SaveChanges();
        }

        public void DeleteQuestion(int id)
        {
            var question = _context.Questions.FirstOrDefault(q => q.Id == id);
            if (question != null)
            {
                _context.Questions.Remove(question);
                _context.SaveChanges();
            }
        }
    }
}
