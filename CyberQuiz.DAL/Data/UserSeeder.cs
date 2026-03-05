using CyberQuiz.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CyberQuiz.DAL.Data
{
    public static class UserSeeder
    {
        public static async Task SeedTestUserAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            //Loggning för att bättre följa seedningen och vad som ev går fel
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("UserSeeder");

            // Kontrollera om användaren redan finns
            var existingUser = await userManager.FindByEmailAsync("user@test.com");
            if (existingUser != null)
            {
                logger.LogInformation("Test user 'user@test.com' already exists with ID: {UserId}", existingUser.Id);
                return;
            }

            // Skapa testanvändare
            var testUser = new ApplicationUser
            {
                UserName = "user@test.com",
                Email = "user@test.com",
                EmailConfirmed = true,
                DisplayName = "Test User",
                IsActive = true
            };
            // sparar user i AspNetUsers-tabellen — UserManager sätter NormalizedEmail automatiskt
            var result = await userManager.CreateAsync(testUser, "Password1234!");

            if (result.Succeeded)
            {
                logger.LogInformation("✅ Test user created successfully!");
                logger.LogInformation("   Username: user");
                logger.LogInformation("   Password: Password1234!");
                logger.LogInformation("   UserId: {UserId}", testUser.Id);
            }
            else
            {
                logger.LogError("❌ Failed to create test user:");
                foreach (var error in result.Errors)
                {
                    logger.LogError("   - {ErrorDescription}", error.Description);
                }
            }
        }
    }
}
