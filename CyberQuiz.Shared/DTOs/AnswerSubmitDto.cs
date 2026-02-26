using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.Shared.DTOs
{
    public class AnswerSubmitDto
    {
        public int QuestionId { get; init; }
        public int SelectedAnswerOptionId { get; init; }
        public string UserId { get; init; } = string.Empty;
    }
}
