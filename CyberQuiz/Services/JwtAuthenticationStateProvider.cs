using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace CyberQuiz.Services
{

    //En custom AuthenticationStateProvider som hanterar autentiseringstillståndet i din Blazor Server-applikation.
    //Den kopplar samman vår JWT-baserade AuthService med Blazor's inbyggda autentiseringssystem.
    //Berättar för Blazor-komponenter vem som är inloggad
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        //AuthService innehåller JWT-token och UserId
        private readonly AuthService _authService;

        public JwtAuthenticationStateProvider(AuthService authService)
        {
            _authService = authService;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //Skapa en tom (ej autentiserad) ClaimsIdentity
            var identity = new ClaimsIdentity();

            //Om användaren är inloggad och har ett giltigt id skapas claims och en autentiserad identitet
            if (_authService.IsLoggedIn && !string.IsNullOrEmpty(_authService.UserId))
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, _authService.UserId),
                    new Claim(ClaimTypes.Name, _authService.UserId)
                };

                identity = new ClaimsIdentity(claims, "jwt");
            }

            //skapa en ClaimsPrincipal som används i Blazor-komponenter "@context.User"
            var user = new ClaimsPrincipal(identity);
            return Task.FromResult(new AuthenticationState(user));
        }

        //Meddela att autentiseringstillståndet ändrats, vilket triggar omrendering av alla komponenter som beror på autentisering
        //Anropas av AuthService vid inlogg, utlogg och registrering
        //En wrappad metod så att basklassens metod kan anropas utifrån av AuthService
        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
