using CyberQuiz.Components;
using CyberQuiz.Services;
using CyberQuiz.UI.Components;
using Microsoft.AspNetCore.Components.Authorization;

//using CyberQuiz.Components;
//using CyberQuiz.Components.Account;
//using CyberQuiz.DAL;
//using CyberQuiz.DAL.Models;
//using CyberQuiz.Services;
//using CyberQuiz.UI.Components;
//using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//HTTP resurs som pekar på API.
// Singleton för att AuthService (Singleton) ska kunna använda den
// Alla sidor som injekterar HttpClient får samma instans som AuthService använder
builder.Services.AddSingleton(sp =>
{
    var httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://localhost:7088/"),
        Timeout = TimeSpan.FromMinutes(5)
    };
    return httpClient;
});

//Authservice . minnet som håller koll på token och userID
// Singleton = samma instans för hela appens livstid, token förloras inte vid navigering
builder.Services.AddSingleton<AuthService>(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    return new AuthService(httpClient);
});

// Lägg till authentication och authorization services
builder.Services.AddAuthentication("CustomScheme")
    .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomScheme", options => { });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider>(sp =>
{
    var authService = sp.GetRequiredService<AuthService>();
    var provider = new JwtAuthenticationStateProvider(authService);
    authService.SetAuthenticationStateProvider(provider);
    return provider;
});



//builder.Services.AddCascadingAuthenticationState();
//builder.Services.AddScoped<IdentityRedirectManager>();
//builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

//builder.Services.AddAuthentication(options =>
//    {
//        options.DefaultScheme = IdentityConstants.ApplicationScheme;
//        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
//    })
//    .AddIdentityCookies();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    //app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    //app.UseExceptionHandler("/Error", createScopeForErrors: true);
    //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


//app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
//app.MapAdditionalIdentityEndpoints();

app.Run();




//UI <-> API(REFERERAR INTE TILL VARANDRA) 
//CORS OCH BASEADRESS HTTP 
//API -> BLL & SHARED(CLASS LIBRARY(DTO) 
//BLL -> DAL & SHARED(CLASS LIBRARY(DTO)
//DAL -> SHARED

//SCENARIO
//Användare som svarar på en fråga
//1. UI -> skickar svaret till API
//2. API -> tar emot svaret och skickar det vidare till BLL(anropa service)
//3. BLL -> tar emot svaret och räknar rätt/fel och skickar det vidare till DAL(3 Services POST api/quiz/answer)
//4. DAL -> tar emot svaret och sparar det i databasen
//5. BLL -> räkna progression
//6. API -> skickar progressionen tillbaka till UI
//7. UI -> visar progressionen för användaren


//UI -> BLL -> DAL
//UI <-> API cors och http adress

//UI -> PAGES
//API -> ENDPOINTS POST GET OSV, POST API/AI/FEEDBACK, GET API/AI/PROGRESS
//BLL -> LOGIK, RÄTT FEL , PROGRESSION, SERVICES 
//DAL -> MIGRATION, DBCONTEXT, MODELLER(ENDAST FÖR DATABAS). 
//SHARED -> DTO, OBJEKT SOM ANVÄNDS MELLAN LAGER, KAN INTE REFERERA TILL DAL, BLL ELLER API.
