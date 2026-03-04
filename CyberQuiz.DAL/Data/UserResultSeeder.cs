using CyberQuiz.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CyberQuiz.DAL.Data
{
    public static class UserResultSeeder
    {
        public static async Task SeedTestUserResultsAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("UserResultSeeder");

            // Hämta testanvändaren
            var testUser = await userManager.FindByNameAsync("user");
            if (testUser == null)
            {
                logger.LogWarning("Test user 'user' not found. Run UserSeeder first.");
                return;
            }

            // Kolla om användaren redan har resultat
            var existingResults = await context.UserResults
                .Where(ur => ur.UserId == testUser.Id)
                .CountAsync();

            if (existingResults > 0)
            {
                logger.LogInformation("Test user already has {Count} quiz results.", existingResults);
                return;
            }

            logger.LogInformation("Seeding quiz results for user '{UserName}'...", testUser.UserName);

            var userResults = new List<UserResult>();
            var answeredAt = DateTime.UtcNow.AddDays(-7);

            // SubCategory 1: Grundläggande Nätverk (80% rätt - 8 av 10 rätt)
            userResults.AddRange(CreateResults(testUser.Id, 1, new[] { 1, 1, 1, 1, 1, 1, 1, 1, 4, 7 }, ref answeredAt));

            // SubCategory 4: OWASP Top 10 (40% rätt - 4 av 10 rätt)
            userResults.AddRange(CreateResults(testUser.Id, 4, new[] { 31, 93, 94, 95, 96, 97, 35, 36, 37, 38 }, ref answeredAt));

            // SubCategory 7: Phishing (60% rätt - 6 av 10 rätt)
            userResults.AddRange(CreateResults(testUser.Id, 7, new[] { 61, 62, 63, 187, 188, 189, 67, 68, 69, 70 }, ref answeredAt));

            // Spara till databasen
            context.UserResults.AddRange(userResults);
            await context.SaveChangesAsync();

            var correctCount = userResults.Count(ur => ur.IsCorrect);
            var totalCount = userResults.Count;
            var successRate = (double)correctCount / totalCount * 100;

            logger.LogInformation("✅ Seeded {Total} quiz results ({Correct} correct, {successRate:F1}%)", 
                totalCount, correctCount, successRate);
        }

        private static List<UserResult> CreateResults(
            string userId, 
            int subCategoryId, 
            int[] answerOptionIds, 
            ref DateTime answeredAt)
        {
            var results = new List<UserResult>();

            foreach (var answerOptionId in answerOptionIds)
            {
                results.Add(new UserResult
                {
                    UserId = userId,
                    QuestionId = GetQuestionIdFromAnswerOption(answerOptionId),
                    AnswerOptionId = answerOptionId,
                    IsCorrect = IsAnswerCorrect(answerOptionId),
                    AnsweredAt = answeredAt
                });

                answeredAt = answeredAt.AddMinutes(2);
            }

            return results;
        }

        private static int GetQuestionIdFromAnswerOption(int answerOptionId)
        {
            return (answerOptionId - 1) / 3 + 1;
        }

        private static bool IsAnswerCorrect(int answerOptionId)
        {
            return (answerOptionId - 1) % 3 == 0;
        }
    }
}
