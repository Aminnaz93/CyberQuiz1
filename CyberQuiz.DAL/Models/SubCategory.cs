using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.DAL.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrderIndex { get; set; }   // för att veta vilken som låser upp nästa

        // FK till Category
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Navigationsproperty 
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
