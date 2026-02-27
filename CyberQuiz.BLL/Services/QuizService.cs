using CyberQuiz.DAL.Models;
using CyberQuiz.DAL.Repositories;
using CyberQuiz.Shared.Constants;
using CyberQuiz.Shared.DTOs;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace CyberQuiz.BLL.Services
{
    public class QuizService : IQuizService
    {
        private readonly QuestionRepository _questionRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly UserResultRepository _userResultRepository;

        public QuizService(QuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        // TODO: Implementera när CategoryRepository är klar i DAL.
        // 1. Hämta alla kategorier med subkategorier och frågor inladdade
        //    via CategoryRepository.GetAllCategoriesWithSubCategoriesAndQuestionsAsync()
        // 2. Mappa till CategoryDto — sätt QuestionCount från antalet frågor per subkategori
        // 3. Sätt IsUnlocked per subkategori genom att anropa IsSubCategoryUnlockedAsync(userId, sc.Id)
        //    (håller logiken på ett ställe istället för att duplicera 80%-beräkningen här)
        public async Task<List<CategoryDto>> GetAllCategoriesAsync(string userId)
        {
            // TODO: Byt till rätt metodnamn när CategoryRepository är klar
            var categories = await _categoryRepository.GetCategoriesWithSubCategoriesAsync();

            var result = new List<CategoryDto>();

            foreach (var category in categories)
            {
                var subCategoryDtos = new List<SubCategoryDto>();

                foreach (var sc in category.SubCategories)
                {
                    var isUnlocked = await IsSubCategoryUnlockedAsync(userId, sc.Id);

                    subCategoryDtos.Add(new SubCategoryDto
                    {
                        Id = sc.Id,
                        Name = sc.Name,
                        OrderIndex = sc.OrderIndex,
                        CategoryId = sc.CategoryId,
                        IsUnlocked = isUnlocked,
                        QuestionCount = sc.Questions.Count
                    });
                }

                result.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    SubCategories = subCategoryDtos
                });
            }

            return result;
        }
        public async Task<List<QuestionDto>> GetQuestionsBySubCategoryAsync(int subCategoryId)
        {
            // Hämtar frågor för subkategorin via repository
        
            var questions = await _questionRepository.GetQuestionBySubcategory(subCategoryId);//Metoden i DAL är inte Async

            // Omvandlar varje Question-modell till QuestionDto
            //Blir nästlad för att sätta Question i en och AnswerOptions i en annan DTO.
            var result = questions.Select(q => new QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
                SubCategoryId = q.SubCategoryId,

                // AnswerOptions kan vara tom lista tills DAL-metoden uppdateras med Include
                AnswerOptions = q.AnswerOptions?.Select(a => new AnswerOptionDto
                {
                    Id = a.Id,
                    Text = a.Text
                    // IsCorrect utelämnas medvetet — säkerhetskrav
                }).ToList() ?? []
            }).ToList();

            return Task.FromResult(result);
        }

        // TODO: Implementera när UserResultRepository och CategoryRepository är klara i DAL.
        // 1. Hämta föregående subkategori (den med närmast lägre OrderIndex inom samma kategori)
        //    via CategoryRepository.GetSubCategoryWithSiblingsAsync(subCategoryId)
        // 2. Hämta användarens svar för den föregående subkategorin
        //    via UserResultRepository.GetResultsByUserAndSubCategoryAsync(userId, föregåendeSubCategoryId)
        // 3. Räkna ut andelen rätta svar — om >= 80% returnera true, annars false
        // 4. Första subkategorin i en kategori är alltid upplåst (ingen föregående att klara)
        // OBS: Progressionen är per användare — lagras inte i DAL utan beräknas här i BLL varje gång
        public async Task<bool> IsSubCategoryUnlockedAsync(string userId, int subCategoryId)
        {
            // TODO: Byt till rätt metodnamn när UserResultRepository är klar
            var userResults = await _userResultRepository.GetResultsByUserAndSubCategoryAsync(userId, subCategoryId);

            if (userResults == null || !userResults.Any())
                return false;

            var correctCount = userResults.Count(r => r.IsCorrect);
            return (double)correctCount / userResults.Count >= QuizConstants.MinPassScore;
        }
    }
}
