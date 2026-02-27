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
            throw new NotImplementedException(); //Implementera
        }

        // Hämtar användarens progression för alla subkategorier —
        // används på profilsidan för att visa en överblick av hur långt användaren kommit
        public async Task<List<ProgressionDto>> GetProgressionByUserAsync(string userId)
        {
           throw new NotImplementedException(); //Implementera
        }
    }
}
