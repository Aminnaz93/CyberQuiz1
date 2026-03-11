using CyberQuiz.Shared.DTOs;
using System.Net.Http.Json;

namespace CyberQuiz.Services
{
    // AuthService håller koll på om användaren är inloggad
    // och sparar token i minnet
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private JwtAuthenticationStateProvider? _authStateProvider;

        // Token sparas här i minnet medan appen körs
        private string? _token;

        // UserId sparas här så andra sidor kan använda den
        public string? UserId { get; private set; }

        // Sant om användaren är inloggad
        public bool IsLoggedIn => _token != null;

        // Event som triggas när inloggningsstatus ändras
        // NavMenu lyssnar på detta för att uppdatera sig
        public event Action? OnAuthStateChanged;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void SetAuthenticationStateProvider(JwtAuthenticationStateProvider authStateProvider)
        {
            _authStateProvider = authStateProvider;
        }

        // Anropas från Login.razor
        // Skickar email + lösenord till API och sparar token om det lyckas
        public async Task<bool> LoginAsync(string email, string password)
        {
            var dto = new LoginDto { Email = email, Password = password };

            var response = await _httpClient.PostAsJsonAsync("api/auth/login", dto);

            if (!response.IsSuccessStatusCode)
                return false;

            // Läs token från API-svaret
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            if (result?.Token == null)
                return false;

            // Spara token och sätt den på alla framtida anrop
            SetToken(result.Token);
            _authStateProvider?.NotifyAuthenticationStateChanged();
            OnAuthStateChanged?.Invoke();
            return true;
        }

        // Anropas från Register.razor
        // Skickar email + lösenord till API och sparar token om det lyckas
        public async Task<(bool Success, string? ErrorMessage)> RegisterAsync(string email, string password, string confirmPassword)
        {
            var dto = new RegisterDto
            {
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/register", dto);

            if (!response.IsSuccessStatusCode)
            {
                // Försök läsa felmeddelande från API:et
                try
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return (false, errorContent);
                }
                catch
                {
                    return (false, "Något gick fel vid registrering.");
                }
            }

            // Läs token från API-svaret
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            if (result?.Token == null)
                return (false, "Ingen token mottogs från servern.");

            // Spara token och sätt den på alla framtida anrop
            SetToken(result.Token);
            _authStateProvider?.NotifyAuthenticationStateChanged();
            OnAuthStateChanged?.Invoke();
            return (true, null);
        }

        // Anropas när användaren loggar ut
        public void Logout()
        {
            _token = null;
            UserId = null;

            // Ta bort token från HttpClient
            _httpClient.DefaultRequestHeaders.Authorization = null;

            _authStateProvider?.NotifyAuthenticationStateChanged();
            OnAuthStateChanged?.Invoke();
        }

        // Sätter token på HttpClient så den skickas med automatiskt
        private void SetToken(string token)
        {
            _token = token;

            // Läs ut userId från token
            UserId = ParseUserIdFromToken(token);

            // Sätt Authorization-headern på HttpClient så den skickas med på varje anrop
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        // Läser ut userId ur JWT-token utan extra paket
        private string? ParseUserIdFromToken(string token)
        {
            try
            {
                // JWT består av tre delar separerade med punkter: header.payload.signature
                var parts = token.Split('.');
                if (parts.Length != 3)
                    return null;

                // Payload är base64-kodad — avkoda den
                var payload = parts[1];

                // Lägg till padding om det behövs
                payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');

                var json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(payload));

                // Hitta nameidentifier i JSON
                var doc = System.Text.Json.JsonDocument.Parse(json);
                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    if (prop.Name.EndsWith("nameidentifier"))
                        return prop.Value.GetString();
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        // Hjälpklass för att läsa token från API-svaret
        private class TokenResponse
        {
            public string? Token { get; set; }
        }
    }
}
