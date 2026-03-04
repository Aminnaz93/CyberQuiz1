namespace CyberQuiz.BLL.Services
{
    public interface IAIService2
    {
        Task<string> AskAsync(string userId, string userMessage);
    }
}
