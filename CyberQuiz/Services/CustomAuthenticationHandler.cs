using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CyberQuiz.Services
{
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        // Denna kopplar ihop AuthService(JWT-baserad Auth) med ASP .NET Core:s inbyggda authentication middleware

        private readonly AuthService _authService;

        public CustomAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            AuthService authService)
            : base(options, logger, encoder)
        {
            _authService = authService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //Kollar om användaren är inloggad via AuthService

            if (!_authService.IsLoggedIn || string.IsNullOrEmpty(_authService.UserId))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, _authService.UserId),
                new Claim(ClaimTypes.Name, _authService.UserId)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            //När en användare försöker nå en sida med [Authorize] utan att vara inloggad skickas den till inloggningssidan
            Context.Response.Redirect("/login");
            return Task.CompletedTask;
        }
    }
}
