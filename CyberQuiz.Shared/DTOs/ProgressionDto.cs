using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.Shared.DTOs
{
    public class ProgressionDto
    {
        public string UserId { get; init; } = string.Empty;
        public int SubCategoryId { get; init; }
        public string SubCategoryName { get; init; } = string.Empty;
        public string CategoryName { get; init; } = string.Empty;
        public bool IsPassed { get; init; }
    }
}
