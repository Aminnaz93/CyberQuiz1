using CyberQuiz.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.DAL.Repositories
{
    public interface IUserResultRepository
    {
        Task<IEnumerable<UserResult>> GetAllUserResultsByUserIdAsync(string userId);
        Task<IEnumerable<UserResult>> GetAllUserResultsAsync();
        Task<UserResult?> GetUserResultByIdAsync(int id);
        Task<IEnumerable<UserResult>> GetUserProgressByUserAndSubcategoryAsync(string userId, int subCategoryId);
        Task AddUserResultAsync(UserResult userResult);
        Task<UserResult?> GetUserResultByUserAndQuestionAsync(string userId, int questionId);
    }
}
