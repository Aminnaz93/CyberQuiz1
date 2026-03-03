using CyberQuiz.DAL.Repositories;
using CyberQuiz.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace CyberQuiz.BLL.Services
{
    public class AIService : IAIService
    {
        private readonly IUserResultRepository _userResultRepository;
        private readonly HttpClient _httpClient;
        private readonly ILogger<AIService> _logger;
        private readonly string _ollamaBaseUrl;
        private readonly string _modelName;

        public AIService(
            IUserResultRepository userResultRepository,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<AIService> logger)
        {
            _userResultRepository = userResultRepository;
            _httpClient = httpClientFactory.CreateClient("Ollama");
            _logger = logger;
            _ollamaBaseUrl = configuration["Ollama:BaseUrl"] ?? "http://localhost:11434";
            _modelName = configuration["Ollama:Model"] ?? "llama3.2";
        }

        public async Task<AIRecommendationResponseDto> GetStudyRecommendationsAsync(
            string userId,
            int? subCategoryId = null)
        {
            // 1. Hämta användarens resultat
            var userResults = await _userResultRepository.GetAllUserResultsByUserIdAsync(userId);

            if (!userResults.Any())
            {
                return new AIRecommendationResponseDto
                {
                    Summary = "Du har inte svarat på några frågor än. Börja quizza för att få personliga rekommendationer!",
                    Recommendations = new List<StudyRecommendationDto>()
                };
            }

            // 2. Filtrera på subkategori om angiven
            if (subCategoryId.HasValue)
            {
                userResults = userResults.Where(ur =>
                    ur.Question.SubCategoryId == subCategoryId.Value);
            }

            // 3. Analysera resultat per subkategori
            var analysis = userResults
                .GroupBy(ur => new
                {
                    ur.Question.SubCategoryId,
                    SubCategoryName = ur.Question.SubCategory?.Name ?? "Okänd kategori"
                })
                .Select(g => new CategoryAnalysis
                {
                    SubCategoryName = g.Key.SubCategoryName,
                    TotalQuestions = g.Count(),
                    CorrectAnswers = g.Count(ur => ur.IsCorrect),
                    IncorrectQuestions = g.Where(ur => !ur.IsCorrect)
                        .Select(ur => new IncorrectQuestionInfo
                        {
                            QuestionText = ur.Question.Text,
                            UserAnswer = ur.AnswerOption?.Text ?? "Inget svar",
                            CorrectAnswer = ur.Question.AnswerOptions
                                .FirstOrDefault(ao => ao.IsCorrect)?.Text ?? "Okänd"
                        })
                        .ToList()
                })
                .OrderBy(a => (double)a.CorrectAnswers / a.TotalQuestions)
                .ToList();

            // 4. Bygg prompt för Ollama
            var prompt = BuildPromptForOllama(analysis);

            // 5. Anropa Ollama
            var aiResponse = await CallOllamaAsync(prompt);

            // 6. Parsa AI-svar och returnera
            return ParseAIResponse(aiResponse, analysis, userResults);
        }

        private string BuildPromptForOllama(List<CategoryAnalysis> analysis)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Du är en expert inom cybersäkerhet och hjälper studenter att förbättra sina kunskaper.");
            sb.AppendLine("Analysera följande quiz-resultat och ge konkreta studieråd:");
            sb.AppendLine();

            foreach (var category in analysis)
            {
                var successRate = (double)category.CorrectAnswers / category.TotalQuestions * 100;
                sb.AppendLine($"Kategori: {category.SubCategoryName}");
                sb.AppendLine($"Resultat: {category.CorrectAnswers}/{category.TotalQuestions} rätt ({successRate:F1}%)");

                if (category.IncorrectQuestions.Count > 0)
                {
                    sb.AppendLine($"Totalt {category.IncorrectQuestions.Count} felaktiga svar.");

                    var examplesToShow = Math.Min(5, category.IncorrectQuestions.Count);
                    sb.AppendLine($"Exempel på {examplesToShow} av dessa:");

                    foreach (var q in category.IncorrectQuestions.Take(examplesToShow))
                    {
                        sb.AppendLine($"  - Fråga: {q.QuestionText}");
                        sb.AppendLine($"    Svarade: {q.UserAnswer} (Rätt: {q.CorrectAnswer})");
                    }
                }
                sb.AppendLine();
            }

            sb.AppendLine("Ge rekommendationer i följande JSON-format:");
            sb.AppendLine(@"{
  ""summary"": ""Övergripande analys på svenska"",
  ""recommendations"": [
    {
      ""topic"": ""Ämnesområde"",
      ""reason"": ""Varför studera detta"",
      ""resources"": [
        {
          ""title"": ""Resurstitel"",
          ""url"": ""https://....."",
          ""description"": ""Beskrivning av resursen"",
          ""type"": ""article""
        }
      ]
    }
  ]
}");

            return sb.ToString();
        }

        private async Task<string> CallOllamaAsync(string prompt)
        {
            var request = new
            {
                model = _modelName,
                prompt = prompt,
                stream = false,
                options = new
                {
                    temperature = 0.6,
                    num_predict = 1500
                }
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    $"{_ollamaBaseUrl}/api/generate",
                    request);

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
                return result?.Response ?? "Kunde inte generera rekommendationer.";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error when calling Ollama API");
                throw new Exception($"Ollama API connection error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error when calling Ollama API");
                throw new Exception($"Ollama API error: {ex.Message}", ex);
            }
        }

        private AIRecommendationResponseDto ParseAIResponse(
            string aiResponse,
            List<CategoryAnalysis> analysis,
            IEnumerable<dynamic> userResults)
        {
            try
            {
                var jsonStart = aiResponse.IndexOf('{');
                var jsonEnd = aiResponse.LastIndexOf('}') + 1;

                if (jsonStart >= 0 && jsonEnd > jsonStart)
                {
                    var jsonString = aiResponse.Substring(jsonStart, jsonEnd - jsonStart);
                    var parsed = JsonSerializer.Deserialize<AIRecommendationResponseDto>(
                        jsonString,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (parsed != null)
                    {
                        parsed.TotalQuestionsAnswered = userResults.Count();
                        parsed.CorrectAnswers = userResults.Count(ur => ur.IsCorrect);
                        parsed.SuccessRate = (double)parsed.CorrectAnswers / parsed.TotalQuestionsAnswered * 100;

                        // Fyll i kategori-analys
                        parsed.AnalyzedCategories = analysis.Select(a => a.SubCategoryName).ToList();
                        parsed.WeakestCategory = analysis.FirstOrDefault()?.SubCategoryName;
                        parsed.StrongestCategory = analysis.LastOrDefault()?.SubCategoryName;

                        return parsed;
                    }

                    _logger.LogWarning("AI returned null after deserialization. Response: {Response}",
                        jsonString.Length > 100 ? jsonString.Substring(0, 100) + "..." : jsonString);
                }
                else
                {
                    _logger.LogWarning("No valid JSON found in AI response. Response: {Response}",
                        aiResponse.Length > 100 ? aiResponse.Substring(0, 100) + "..." : aiResponse);
                }
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "Failed to deserialize AI response as JSON. Using fallback.");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid string operation while parsing AI response.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while parsing AI response.");
            }

            // Fallback: returnera enkel analys
            _logger.LogInformation("Using fallback response format");
            return new AIRecommendationResponseDto
            {
                Summary = aiResponse,
                TotalQuestionsAnswered = userResults.Count(),
                CorrectAnswers = userResults.Count(ur => ur.IsCorrect),
                SuccessRate = (double)userResults.Count(ur => ur.IsCorrect) / userResults.Count() * 100,
                Recommendations = new List<StudyRecommendationDto>()
            };
        }

        public async Task<bool> HealthCheckAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_ollamaBaseUrl}/api/tags");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Ollama health check failed");
                return false;
            }
        }

        private class OllamaResponse
        {
            public string Response { get; set; } = string.Empty;
        }

        private class CategoryAnalysis
        {
            public string SubCategoryName { get; set; } = string.Empty;
            public int TotalQuestions { get; set; }
            public int CorrectAnswers { get; set; }
            public List<IncorrectQuestionInfo> IncorrectQuestions { get; set; } = new();
        }

        private class IncorrectQuestionInfo
        {
            public string QuestionText { get; set; } = string.Empty;
            public string UserAnswer { get; set; } = string.Empty;
            public string CorrectAnswer { get; set; } = string.Empty;
        }
    }
}
