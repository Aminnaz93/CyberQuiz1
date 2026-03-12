using CyberQuiz.DAL.Models;
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
            var categories = await _categoryRepository.GetAllCategoriesWithSubCategoriesAsync();

            // Hämta allt vi behöver EN GÅNG innan loopen, annars blir det DB-anrop per subkategori
            var allUserResults = await _userResultRepository.GetAllUserResultsByUserIdAsync(userId);
            var allSubCategoriesWithQuestions = await _subCategoryRepository.GetAllSubCategoriesWithQuestionsAsync();

            // Sparar antal frågor per subkategori i en dictionary (Id → antal frågor)
            // så att vi kan slå upp det snabbt i loopen utan extra DB-anrop
            //Kan egenligen i vårt fall direkt sätta till 10 frågor/kategori men om det skulle variera eller ändras så kollas det upp!
            var questionCountLookup = allSubCategoriesWithQuestions
                .ToDictionary(sc => sc.Id, sc => sc.Questions?.Count ?? 0);

            var result = new List<CategoryDto>();
            //Loopar genom för att sätta categorier i en DTO och subkategorier i en DTO
            foreach (var category in categories)
            {
                var subCategoryDtos = new List<SubCategoryDto>();

                // Sorterar på OrderIndex så att ordningen alltid är rätt
                var sortedSubCategories = category.SubCategories.OrderBy(sc => sc.OrderIndex).ToList();

                foreach (var sc in sortedSubCategories)
                {
                    // Kollar upplåsning med redan hämtad data - inga extra DB-anrop
                    var isUnlocked = IsSubCategoryUnlockedFromData(sc.Id, sortedSubCategories, allUserResults);

                    subCategoryDtos.Add(new SubCategoryDto
                    {
                        Id = sc.Id,
                        Name = sc.Name,
                        OrderIndex = sc.OrderIndex,
                        CategoryId = sc.CategoryId,
                        IsUnlocked = isUnlocked,
                        QuestionCount = questionCountLookup.GetValueOrDefault(sc.Id, 0) //0 ifall den inte skulle hitta id
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

        //DEN GAMLA METODEN - ANVÄNDS INTE LÄNGRE!
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

            // Gruppera på AttemptId och hitta bästa genomförda quiz-omgång
            var attemptsWithId = userResults
                .Where(r => r.AttemptId.HasValue)
                .GroupBy(r => r.AttemptId)
                .ToList();

            if (attemptsWithId.Any())
            {
                // Bästa omgång = den med högst andel rätt svar
                var bestScore = attemptsWithId
                    .Select(g => (double)g.Count(r => r.IsCorrect) / g.Count())
                    .Max();
                if (bestScore >= QuizConstants.MinPassScore)
                    return true;
            }

            // Kolla även äldre data utan AttemptId (sparad innan AttemptId introducerades).
            // Viktigt: detta måste köras även om det finns AttemptId-data, annars ignoreras
            // gamla godkända försök när användaren misslyckas med ett nytt quiz.
            var seededResults = userResults.Where(r => !r.AttemptId.HasValue).ToList();
            if (seededResults.Any())
            {
                var latestPerQuestion = seededResults
                    .GroupBy(r => r.QuestionId)
                    .Select(g => g.OrderByDescending(r => r.AnsweredAt).First())
                    .ToList();

                var correctCount = latestPerQuestion.Count(r => r.IsCorrect);
                return (double)correctCount / latestPerQuestion.Count >= QuizConstants.MinPassScore;
            }

            return false;
        }

        // Samma logik som IsSubCategoryUnlockedAsync men arbetar på redan hämtad data för att slippa så många DB-anrop...
        private bool IsSubCategoryUnlockedFromData(int subCategoryId, List<SubCategory> siblingsInCategory, IEnumerable<UserResult> allUserResults)
        {
            // Första subkategorin är alltid upplåst
            if (siblingsInCategory.First().Id == subCategoryId)
                return true;

            //Sätter nuvarande kategori om det inte är den första!
            var currentSubCategory = siblingsInCategory.FirstOrDefault(sc => sc.Id == subCategoryId);
            if (currentSubCategory == null)
                return false;

            // Hitta föregående subkategori
            var previousSubCategory = siblingsInCategory
                .LastOrDefault(sc => sc.OrderIndex < currentSubCategory.OrderIndex);

            if (previousSubCategory == null)
                return true;

            // Filtrera användarens resultat till föregående subkategori (i minnet, inte DB)
            var userResults = allUserResults
                .Where(r => r.Question?.SubCategoryId == previousSubCategory.Id)
                .ToList();

            if (!userResults.Any())
                return false;

            // Kolla bästa omgång med AttemptId - bästa omgång vinner
            var attemptsWithId = userResults
                .Where(r => r.AttemptId.HasValue)
                .GroupBy(r => r.AttemptId)
                .ToList();

            if (attemptsWithId.Any())
            {
                //Hittar bästa resultatet
                var bestScore = attemptsWithId
                    .Select(g => (double)g.Count(r => r.IsCorrect) / g.Count())
                    .Max();
                if (bestScore >= QuizConstants.MinPassScore)
                    return true;
            }

            // Kolla även äldre data utan AttemptId (sparad innan AttemptId introducerades).
            // Viktigt: detta måste köras även om det finns AttemptId-data, annars ignoreras
            // gamla godkända försök när användaren misslyckas med ett nytt quiz.
            var seededResults = userResults.Where(r => !r.AttemptId.HasValue).ToList();
            if (seededResults.Any())
            {
                var latestPerQuestion = seededResults
                    .GroupBy(r => r.QuestionId)
                    .Select(g => g.OrderByDescending(r => r.AnsweredAt).First())
                    .ToList();

                var correctCount = latestPerQuestion.Count(r => r.IsCorrect);
                return (double)correctCount / latestPerQuestion.Count >= QuizConstants.MinPassScore;
            }

            return false;
        }
    }
}
