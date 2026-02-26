using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.DAL.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }

        // FK till SubCategory
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        // Navigationspropertys
        public ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
        public ICollection<UserResult> UserResults { get; set; } = new List<UserResult>();
    }
}
