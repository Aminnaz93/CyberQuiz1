using System.Security.Claims;
using System.Text;
using CyberQuiz.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace CyberQuiz.BLL.Services
{
    //OBS!
    //Metoderna är tagna från vad som sker på de "färdiga" Pages i UI - några smått modifierade!!!
    //Några en aning oklara (för mig) men behödes egentligen bara flyttas!
    public class UserService : IUserService 
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender<ApplicationUser> _emailSender; //Kommer vi använda IEmailSender???

        public UserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender<ApplicationUser> emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }


        //Home & Login -->
        public async Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure)
        {
            // SignInManager.PasswordSignInAsync kräver UserName, inte email.
            // Slå upp användaren via email och använd deras UserName för inloggning.
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return SignInResult.Failed;

            // API använder JWT, inte cookies — SignInManager kan inte användas direkt.
            // Validera lösenordet med UserManager istället.
            var passwordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!passwordValid)
                return SignInResult.Failed;

            return SignInResult.Success;
        }

        //Login.razor -- med passkey (WebAuthn-credential i JSON-format) ??? - metoden tagen från Login.razor
        public async Task<SignInResult> PasskeySignInAsync(string credentialJson)
        {
            return await _signInManager.PasskeySignInAsync(credentialJson);
        }


        //Register.Razor
        public async Task<IdentityResult> CreateUserAsync(string email, string password)
        {
            var user = new ApplicationUser();

            // UserName och Email sätts till samma värde (e-postadressen)
            await _userManager.SetUserNameAsync(user, email);
            await _userManager.SetEmailAsync(user, email);

            return await _userManager.CreateAsync(user, password);
        }

        // Register.razor -->
        // Genererar en e-postbekräftelsetoken för den angivna användaren.
        // Tokenen Base64Url-kodas så att den kan skickas säkert i en URL.
        public async Task<string> GenerateEmailConfirmationTokenAsync(string email) //Kommer denna användas???
        {
            var user = await GetUserByEmailOrThrowAsync(email);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Kodar tokenen till Base64Url så att den inte förstörs av URL-encoding
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        }

        // Register.razor -- >
        // Skickar en bekräftelselänk till användarens e-post efter registrering.
        public async Task SendConfirmationLinkAsync(string email, string confirmationLink) //Kommer denna användas???
        {
            var user = await GetUserByEmailOrThrowAsync(email);
            await _emailSender.SendConfirmationLinkAsync(user, email, confirmationLink);
        }

        //Register.razor -->
        // Loggar in en nyregistrerad användare direkt utan lösenord.
        public async Task SignInAfterRegisterAsync(string email, bool isPersistent)
        {
            var user = await GetUserByEmailOrThrowAsync(email);
            await _signInManager.SignInAsync(user, isPersistent);
        }

        // Returnerar om e-postbekräftelse krävs för att logga in.
        // Styr om Register-sidan ska omdirigera till RegisterConfirmation eller logga in direkt.
        //Ska vi ha denna???
        public bool RequireConfirmedAccount =>
            _userManager.Options.SignIn.RequireConfirmedAccount;


        //ChangePassword.razor -->
        // Hämtar användarens Id - Returnerar null om användaren inte hittas.
        public async Task<string?> GetUserIdFromPrincipalAsync(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            return user?.Id;
        }

        //ChangePassword.razor
        // Kontrollerar om användaren har ett lösenord satt.
        // Om inte ska sidan omdirigera till SetPassword istället - för att inte krascha
        //Behövs denna???
        public async Task<bool> HasPasswordAsync(string userId)
        {
            var user = await GetUserByIdOrThrowAsync(userId);
            return await _userManager.HasPasswordAsync(user);
        }

        //ChangePassword.razor --> byter lösenord
        // Byter lösenord för användaren. Kräver att det gamla lösenordet stämmer.
        // Returnerar IdentityResult med eventuella felmeddelanden vid misslyckande.
        public async Task<IdentityResult> ChangePasswordAsync(
            string userId, string oldPassword, string newPassword)
        {
            var user = await GetUserByIdOrThrowAsync(userId);
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        //ChangePassword.razor -->
        // Uppdaterar användarens sign-in-cookie efter lösenordsbytet
        public async Task RefreshSignInAsync(string userId)
        {
            var user = await GetUserByIdOrThrowAsync(userId);
            await _signInManager.RefreshSignInAsync(user);
        }
        
        //Email.razor
        // Hämtar den e-postadress som är registrerad på kontot.
        public async Task<string?> GetEmailAsync(string userId)
        {
            var user = await GetUserByIdOrThrowAsync(userId);
            return await _userManager.GetEmailAsync(user);
        }

        //Email.razor -->
        // Kontrollerar om användarens e-post är bekräftad.
        // Styr om en bock eller en "skicka verifiering"-knapp visas i Email.razor???
        public async Task<bool> IsEmailConfirmedAsync(string userId)
        {
            var user = await GetUserByIdOrThrowAsync(userId);
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        //Email.razor -->
        // Genererar en token för att byta e-post till newEmail.
        // Tokenen Base64Url-kodas för säker överföring i URL.
        public async Task<string> GenerateChangeEmailTokenAsync(string userId, string newEmail)
        {
            var user = await GetUserByIdOrThrowAsync(userId);

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);

            // Kodar tokenen till Base64Url så att den inte förstörs av URL-encoding
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        }

        //Email.razor -->
        // Skickar en bekräftelselänk till den nya e-postadressen.
        // Användaren måste klicka på länken för att bekräfta bytet.
        //Behöver vi denna???
        public async Task SendEmailChangeLinkAsync(string userId, string newEmail, string confirmationLink)
        {
            var user = await GetUserByIdOrThrowAsync(userId);
            await _emailSender.SendConfirmationLinkAsync(user, newEmail, confirmationLink);
        }

        //Email.razor -->
        // Skickar ett verifieringsmail till användarens nuvarande e-post.
        // Används om e-posten inte är bekräftad sedan tidigare.
        //Kanske inte opimal i detta projekt
        public async Task SendEmailVerificationAsync(string userId, string verificationLink)
        {
            var user = await GetUserByIdOrThrowAsync(userId);
            var email = await _userManager.GetEmailAsync(user)
                ?? throw new InvalidOperationException($"Användare med id '{userId}' saknar e-postadress.");

            await _emailSender.SendConfirmationLinkAsync(user, email, verificationLink);
        }

        // ChangeEmail --> Enklare för detta projekt! Verifierar lösenord och byter e-post direkt
        public async Task<IdentityResult> ChangeEmailAsync(string userId, string currentPassword, string newEmail)
        {
            var user = await GetUserByIdOrThrowAsync(userId);

            // Verifiera att lösenordet stämmer innan bytet genomförs
            var passwordCorrect = await _userManager.CheckPasswordAsync(user, currentPassword);
            if (!passwordCorrect)
                return IdentityResult.Failed(new IdentityError { Description = "Fel lösenord." });

            // Generera token och genomför bytet direkt
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            return await _userManager.ChangeEmailAsync(user, newEmail, token);
        }

        // ------ PRIVATA HJÄLPMETODER --------

        // Hämtar en användare via e-post. Kastar undantag om användaren inte hittas,
        // eftersom ett null-värde här alltid indikerar ett programmeringsfel.
        private async Task<ApplicationUser> GetUserByEmailOrThrowAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email)
                ?? throw new InvalidOperationException($"Användare med e-post '{email}' hittades inte.");
        }

        // Hämtar en användare via id. Kastar undantag om användaren inte hittas.
        private async Task<ApplicationUser> GetUserByIdOrThrowAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId)
                ?? throw new InvalidOperationException($"Användare med id '{userId}' hittades inte.");
        }
    }
}
