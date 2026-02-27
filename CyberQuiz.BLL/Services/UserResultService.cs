using CyberQuiz.DAL.Models;
using CyberQuiz.DAL.Repositories;
using CyberQuiz.Shared.Constants;
using CyberQuiz.Shared.DTOs;

namespace CyberQuiz.BLL.Services
{
    public class UserResultService : IUserResultService
    {
        private readonly IUserResultRepository _userResultRepository;
        private readonly IQuestionRepository _questionRepository;

        public UserResultService(
            IUserResultRepository userResultRepository,
            IQuestionRepository questionRepository)
        {
            _userResultRepository = userResultRepository;
            _questionRepository = questionRepository;
        }

        // Tar emot användarens svar, sparar det och returnerar om det var rätt
        // samt vilket svarsalternativ som var korrekt
        public async Task<AnswerResultDto> SubmitAnswerAsync(AnswerSubmitDto answerSubmit)
        {
            // Hämta frågan med svarsalternativ för att kunna kolla IsCorrect och hitta rätt svar
            var question = await _questionRepository.GetQuestionByIdAsync(answerSubmit.QuestionId);
            if (question == null)
                throw new ArgumentException($"Fråga med id {answerSubmit.QuestionId} hittades inte.");

            // Kolla om det valda svarsalternativet är korrekt
            var selectedOption = question.AnswerOptions
                .FirstOrDefault(a => a.Id == answerSubmit.SelectedAnswerOptionId);
            if (selectedOption == null)
                throw new ArgumentException($"Svarsalternativ med id {answerSubmit.SelectedAnswerOptionId} hittades inte.");

            // Hitta det korrekta svarsalternativet
            var correctOption = question.AnswerOptions.First(a => a.IsCorrect);

            // Spara användarens svar i databasen
            var userResult = new UserResult
            {
                UserId = answerSubmit.UserId,
                QuestionId = answerSubmit.QuestionId,
                AnswerOptionId = answerSubmit.SelectedAnswerOptionId,
                IsCorrect = selectedOption.IsCorrect,
                AnsweredAt = DateTimeOffset.UtcNow
            };
            await _userResultRepository.AddUserResultAsync(userResult);

            return new AnswerResultDto
            {
                IsCorrect = selectedOption.IsCorrect,
                CorrectAnswerOptionId = correctOption.Id
            };
        }

        // Hämtar användarens progression för alla subkategorier
        // För överblick på typ startsidan eller liknande
        public async Task<List<ProgressionDto>> GetProgressionByUserAsync(string userId)
        {
            // Hämtar alla resultat för användaren, inklusive fråga (för att komma åt SubCategoryId)
            var userResults = await _userResultRepository.GetAllUserResultsByUserIdAsync(userId);

            // Grupperar per subkategori och räknar ut om användaren nått >= 80% rätt
            //Ska returnera användarId, SubCategoryId och om kategorin är klar (>= 80% rätt) eller inte 
            var progression = userResults
                .GroupBy(ur => ur.Question.SubCategoryId)
                .Select(group => new ProgressionDto
                {
                    UserId = userId,
                    SubCategoryId = group.Key,
                    IsPassed = (double)group.Count(r => r.IsCorrect) / group.Count() >= QuizConstants.MinPassScore
                })
                .ToList();

            return progression;
            
        }
    }
}
