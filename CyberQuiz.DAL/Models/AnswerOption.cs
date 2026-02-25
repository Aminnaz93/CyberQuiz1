using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.DAL.Models
{
    internal class AnswerOption
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

        // FK till Question
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
