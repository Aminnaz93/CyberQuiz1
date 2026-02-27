using CyberQuiz.DAL.Repositories;
using CyberQuiz.Shared.Constants;
using CyberQuiz.Shared.DTOs;

namespace CyberQuiz.BLL.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IUserResultRepository _userResultRepository;

        public QuizService(
            IQuestionRepository questionRepository,
            ICategoryRepository categoryRepository,
            ISubCategoryRepository subCategoryRepository,
            IUserResultRepository userResultRepository)
        {
            _questionRepository = questionRepository;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _userResultRepository = userResultRepository;
        }

        // Hämtar alla KATEGORIER
        //- Inlusive SUBKATEGORIER
        //- I NÄSTLADE DTOs!
        public async Task<List<CategoryDto>> GetAllCategoriesAsync(string userId)
        {
            // Hämtar kategorier med subkategorier genom Include i DAL
            var categories = await _categoryRepository.GetAllCategoriesWithSubCategoriesAsync();

            var result = new List<CategoryDto>();

            //Loopar igenom kategorier och subkategorier för att skapa DTOs och kolla upplåsning
            //Lägger i vars en DTO-lista för att skicka till frontend
            foreach (var category in categories)
            {
                var subCategoryDtos = new List<SubCategoryDto>();

                // Sorterar subkategorier på OrderIndex så att ordningen alltid är rätt
                var sortedSubCategories = category.SubCategories.OrderBy(sc => sc.OrderIndex).ToList();

                foreach (var sc in sortedSubCategories)
                {
                    //Kollar om subkategorin är upplåst för den aktuella användaren 
                    var isUnlocked = await IsSubCategoryUnlockedAsync(userId, sc.Id);

                    // Hämta med frågor för att kunna räkna antal frågor i subkategorin (för att visa i UI)
                    var subCategoryWithQuestions = await _subCategoryRepository.GetSubCategoryWithQuestionsAsync(sc.Id);

                    subCategoryDtos.Add(new SubCategoryDto
                    {
                        Id = sc.Id,
                        Name = sc.Name,
                        OrderIndex = sc.OrderIndex,
                        CategoryId = sc.CategoryId,
                        IsUnlocked = isUnlocked,
                        QuestionCount = subCategoryWithQuestions?.Questions?.Count ?? 0 //Antal frågor i subkategorin, 0 om null
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


        //Hämtar alla FRÅGOR för EN SUBKATEGORI
        //- Inklusive SVARSALTERNATIV
        //- skickar INTE med IsCorrect i AnswerOptionDto av säkerhetsskäl (för att inte avslöja rätt svar i DevTools)
        public async Task<List<QuestionDto>> GetAllQuestionsBySubCategoryAsync(int subCategoryId)
        {
            //Hämta alla frågor utifrån subkategori-id, inklusive svarsalternativ
            var questions = await _questionRepository.GetQuestionBySubcategoryAsync(subCategoryId); //Har den include???

            //Nästlade DTOS för att skicka till frontend, där varje fråga har en lista med svarsalternativ
            var result = questions.Select(q => new QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
                SubCategoryId = q.SubCategoryId,

                
                AnswerOptions = q.AnswerOptions?.Select(a => new AnswerOptionDto
                {
                    Id = a.Id,
                    Text = a.Text
                    // IsCorrect utelämnas medvetet — säkerhetskrav
                }).ToList() ?? []
            }).ToList();

            return result;
        }

        // Kontrollerar om en subkategori är upplåst för en användare.
        // Första subkategorin i en kategori ska alltid vara upplåst.
        // Övriga kräver att föregående subkategori är klarad med >= 80% rätt.
        public async Task<bool> IsSubCategoryUnlockedAsync(string userId, int subCategoryId)
        {
            // Hämta den aktuella subkategorin för att veta CategoryId och OrderIndex
            var currentSubCategory = await _subCategoryRepository.GetSubCategoryByIdAsync(subCategoryId);
            if (currentSubCategory == null)
                return false;

            // Hämta alla subkategorier i samma kategori --> sorterade på OrderIndex
            var siblingsInCategory = (await _subCategoryRepository
                .GetSubCategoriesByCategoryIdAsync(currentSubCategory.CategoryId))
                .OrderBy(sc => sc.OrderIndex)
                .ToList();

            // Kolla om första subkategorin stämmer med inskickat id --> i så fall är den alltid upplåst
            if (siblingsInCategory.First().Id == subCategoryId)
                return true;

            // Hitta föregående subkategori --> den med närmast lägre OrderIndex
            var previousSubCategory = siblingsInCategory
                .LastOrDefault(sc => sc.OrderIndex < currentSubCategory.OrderIndex);

            if (previousSubCategory == null)
                return true;

            // Hämta användarens resultat för föregående subkategori
            var userResults = await _userResultRepository
                .GetUserProgressByUserAndSubcategoryAsync(userId, previousSubCategory.Id);

            if (userResults == null || !userResults.Any())
                return false;

            // Räkna ut om användaren nått >= 80% rätt på föregående subkategori
            var resultList = userResults.ToList();
            var correctCount = resultList.Count(r => r.IsCorrect);
            return (double)correctCount / resultList.Count >= QuizConstants.MinPassScore;
        }
    }
}
