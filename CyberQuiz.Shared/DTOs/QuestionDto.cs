using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.Shared.DTOs
{
    public class QuestionDto
    {
        public int Id { get; init; } //init för att inte kunna sätta ett nytt värde på det efter skapande!
        public string Text { get; init; } = string.Empty; //Empty string för att undvika null-värden
        public int SubCategoryId { get; init; }
        public List<AnswerOptionDto> AnswerOptions { get; init; } = [];
    }
}
