namespace CyberQuiz.DAL.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigationsproperty för att hitta alla tillhörande subkategorier
        public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
    }
}

