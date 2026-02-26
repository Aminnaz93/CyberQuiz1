using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.Shared.DTOs
{
    public class CategoryDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public List<SubCategoryDto> SubCategories { get; init; } = [];
    }
}
