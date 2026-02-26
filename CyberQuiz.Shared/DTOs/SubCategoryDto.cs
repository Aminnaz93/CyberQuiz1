using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.Shared.DTOs
{
    public class SubCategoryDto
    {
        public int Id { get; init; } //Init för att man inte ska kunna sätta ett nytt värde på det efter skapande!
        public string Name { get; init; } = string.Empty;
        public int OrderIndex { get; init; }
        public int CategoryId { get; init; }
        public bool IsUnlocked { get; init; }
    }
}
