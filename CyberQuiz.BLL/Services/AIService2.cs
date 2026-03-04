using CyberQuiz.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CyberQuiz.BLL.Services
{
    public class AIService2 : IAIService2
    {
        private readonly IUserResultRepository _userResultRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly HttpClient _httpClient;
        private readonly ILogger<AIService2> _logger;
        private readonly string _ollamaBaseUrl;
        private readonly string _modelName;

        public AIService2(
            IUserResultRepository userResultRepository,
            IQuestionRepository questionRepository,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<AIService2> logger)
        {
            _userResultRepository = userResultRepository;
            _questionRepository = questionRepository;
            _httpClient = httpClientFactory.CreateClient("Ollama");
            _logger = logger;
            _ollamaBaseUrl = configuration["Ollama:BaseUrl"] ?? "http://localhost:11434";
            _modelName = configuration["Ollama:Model"] ?? "llama3.2";
        }

        public async Task<string> AskAsync(string userId, string userMessage)
        {
            _logger.LogInformation("AskAsync called for userId: {UserId}", userId);

            var userResults = await _userResultRepository.GetAllUserResultsByUserIdAsync(userId);

            var messages = new List<OllamaChatMessage>
            {
                new OllamaChatMessage
                {
                    Role = "system",
                    Content = BuildSystemPrompt(userResults)
                },
                new OllamaChatMessage
                {
                    Role = "user",
                    Content = userMessage
                }
            };

            var request = new OllamaChatRequest
            {
                Model = _modelName,
                Messages = messages,
                Stream = false
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    $"{_ollamaBaseUrl}/api/chat",
                    request);

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<OllamaChatResponse>();
                return result?.Message?.Content ?? "Kunde inte generera ett svar.";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error when calling Ollama chat API");
                throw new Exception($"Kunde inte nå AI-tjänsten: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error when calling Ollama chat API");
                throw new Exception($"Ett fel uppstod: {ex.Message}", ex);
            }
        }

        private string BuildSystemPrompt(IEnumerable<dynamic> userResults)
        {
            var sb = new StringBuilder();
            sb.AppendLine("You are a helpful cybersecurity assistant. Answer in the same language the user writes in.");

            if (userResults.Any())
            {
                int total = userResults.Count();
                int correct = userResults.Count(ur => ur.IsCorrect);
                double rate = (double)correct / total * 100;

                sb.AppendLine($"The user has completed {total} quiz questions and answered {correct} correctly ({rate:F0}%).");
                sb.AppendLine("You may reference this if it is relevant to the user's question.");
            }

            return sb.ToString();
        }

        private class OllamaChatMessage
        {
            [JsonPropertyName("role")]
            public string Role { get; set; } = string.Empty;

            [JsonPropertyName("content")]
            public string Content { get; set; } = string.Empty;
        }

        private class OllamaChatRequest
        {
            [JsonPropertyName("model")]
            public string Model { get; set; } = string.Empty;

            [JsonPropertyName("messages")]
            public List<OllamaChatMessage> Messages { get; set; } = new();

            [JsonPropertyName("stream")]
            public bool Stream { get; set; }
        }

        private class OllamaChatResponse
        {
            [JsonPropertyName("message")]
            public OllamaChatMessage? Message { get; set; }
        }
    }
}
