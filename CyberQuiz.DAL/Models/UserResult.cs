using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.DAL.Models
{
    public class UserResult
    {
        public int Id { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime AnsweredAt { get; set; }

        // FK till Identity
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        // FK till Question
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        // FK till valt svar
        public int AnswerOptionId { get; set; }
        public AnswerOption AnswerOption { get; set; }
    }
}
