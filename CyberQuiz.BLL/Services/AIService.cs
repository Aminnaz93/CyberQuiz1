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

        //Dependency injection av services
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
            _logger.LogInformation("GetStudyRecommendationsAsync called for userId: {UserId}", userId);

            // 1. Hämta användarens resultat
            var userResults = await _userResultRepository.GetAllUserResultsByUserIdAsync(userId);
            _logger.LogInformation("Retrieved {Count} user results", userResults.Count());

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
                _logger.LogInformation("Filtered to {Count} results for subCategoryId: {SubCategoryId}", 
                    userResults.Count(), subCategoryId.Value);
            }

            // 3. Analysera resultat per subkategori baserat på antal svarade frågor och andel rätt svar
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
                    // Lista på felaktiga svar (max 3 visas i svarsprompten)
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
                // Sorterar från sämst till bäst (för att identifiera svaga områden först)
                .OrderBy(a => (double)a.CorrectAnswers / a.TotalQuestions)
                .ToList();

            _logger.LogInformation("Analyzed {Count} categories", analysis.Count);

            // 4. Bygg prompt för Ollama
            var prompt = BuildPromptForOllama(analysis);
            _logger.LogInformation("Built prompt, length: {Length} characters", prompt.Length);

            // 5. Anropa Ollama
            _logger.LogInformation("Calling Ollama API...");
            var aiResponse = await CallOllamaAsync(prompt);
            _logger.LogInformation("Ollama responded with {Length} characters", aiResponse.Length);

            // 6. Parsa AI-svar och returnera
            var result = ParseAIResponse(aiResponse, analysis, userResults);
            _logger.LogInformation("Returning recommendations with {Count} items", result.Recommendations.Count);

            return result;
        }

        private string BuildPromptForOllama(List<CategoryAnalysis> analysis)
        {
            //Det blev konstiga svar med svenska, så det fick bli engelska istället
            var sb = new StringBuilder();
            sb.AppendLine("You are a cybersecurity expert. Analyze quiz results and provide study recommendations.");
            sb.AppendLine();
            sb.AppendLine("Quiz Results:");

            foreach (var category in analysis)
            {
                var successRate = (double)category.CorrectAnswers / category.TotalQuestions * 100;
                sb.AppendLine($"- {category.SubCategoryName}: {category.CorrectAnswers}/{category.TotalQuestions} ({successRate:F0}%)");

                if (category.IncorrectQuestions.Count > 0 && category.IncorrectQuestions.Count <= 3)
                {
                    sb.AppendLine($"  Mistakes:");
                    //analysera max 3 felaktiga svar, för att inte prompten ska bli för lång
                    foreach (var q in category.IncorrectQuestions.Take(3))
                    {
                        sb.AppendLine($"  * {q.QuestionText}");
                    }
                }
            }

            sb.AppendLine();
            sb.AppendLine("Generate valid JSON with ONLY these fields:");

            //tydlig instruktion hur svaret ska vara formatterat
            sb.AppendLine(@"{
  ""summary"": ""2-3 sentences analyzing strengths and weaknesses"",
  ""recommendations"": [
    {
      ""topic"": ""Phishing Detection"",
      ""reason"": ""Only 33% success rate indicates need for improvement"",
      ""recommendedResources"": [
        {
          ""title"": ""OWASP Phishing Guide"",
          ""url"": ""https://owasp.org/phishing"",
          ""description"": ""Learn to identify phishing attacks"",
          ""type"": ""article""
        }
      ],
      ""keyConceptsToFocus"": [""Email verification"", ""URL inspection"", ""Social engineering tactics""]
    }
  ]
}");
            sb.AppendLine();
            sb.AppendLine("RULES:");
            sb.AppendLine("1. Use ONLY the fields shown above (topic, reason, recommendedResources, keyConceptsToFocus)");
            sb.AppendLine("2. Generate 2-3 recommendations with real URLs");
            sb.AppendLine("3. Return ONLY valid JSON, no extra text");

            return sb.ToString();
        }
        //Ollama API anrop
        private async Task<string> CallOllamaAsync(string prompt)
        {
            var request = new
            {
                model = _modelName,
                prompt = prompt,
                stream = false,
                format = "json",
                options = new
                {
                    temperature = 0.3,    // Lägre = mer deterministisk JSON Inte så utsvävande svar
                    num_predict = 1200,   // Ökat för att hinna generera hela JSON-svaret
                    top_k = 40,           // Begränsar vokabulär (snabbare)
                    top_p = 0.9           // bortser från de minst sannolika orden
                }
            };

            try
            {
                _logger.LogInformation("Sending request to Ollama with format=json, timeout=300s");
                var response = await _httpClient.PostAsJsonAsync(
                    $"{_ollamaBaseUrl}/api/generate",
                    request);

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
                _logger.LogInformation("Ollama raw response (first 200 chars): {Response}", 
                    result?.Response?.Length > 200 ? result.Response.Substring(0, 200) : result?.Response);

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

        //Hantera svaret från Ollama
        private AIRecommendationResponseDto ParseAIResponse(
            string aiResponse,
            List<CategoryAnalysis> analysis,
            IEnumerable<dynamic> userResults)
        {
            try
            {
                //För att undvika att få med inledande eller avslutande artighetsfraser o dyl
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
                        //lägg till lite statistik
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
        //Testmetod att kalla innan man gör det riktiga anropet
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
