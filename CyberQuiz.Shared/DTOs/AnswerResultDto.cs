using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.Shared.DTOs
{
    public class AnswerResultDto
    {
        public bool IsCorrect { get; init; }
        public int CorrectAnswerOptionId { get; init; }
    }
}
