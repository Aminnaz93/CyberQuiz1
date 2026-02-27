using CyberQuiz.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.DAL.Repositories
{
    public interface ISubCategoryRepository
    {
        Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(int categoryId);
        Task<IEnumerable<SubCategory>> GetAllSubCategoriesAsync();
        Task<SubCategory?> GetSubCategoryByIdAsync(int id);
        Task<IEnumerable<SubCategory>> GetAllSubCategoriesWithQuestionsAsync();
        Task<SubCategory?> GetSubCategoryWithQuestionsAsync(int id);
    }
}
