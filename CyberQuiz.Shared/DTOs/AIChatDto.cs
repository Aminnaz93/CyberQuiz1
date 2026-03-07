namespace CyberQuiz.Shared.DTOs
{
    public class AIChatRequestDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class AIChatResponseDto
    {
        public string Message { get; set; } = string.Empty;
    }
}