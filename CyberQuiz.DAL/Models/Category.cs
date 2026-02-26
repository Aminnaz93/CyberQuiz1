using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.DAL.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigationsproperty
        public ICollection<SubCategory> SubCategories { get; set; }
    }
}

