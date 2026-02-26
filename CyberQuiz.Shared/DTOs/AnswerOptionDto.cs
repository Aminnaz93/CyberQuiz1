using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.Shared.DTOs
{
    public class AnswerOptionDto
    {
        public int Id { get; init; } //Init för att man inte ska kunna sätta ett nytt värde på det efter skapande!
        public string Text { get; init; } = string.Empty;
        // IsCorrect utelämnas för att man inte ska kunna gå in i DevTools och kolla svar –> säkerhetskrav
    }
}
