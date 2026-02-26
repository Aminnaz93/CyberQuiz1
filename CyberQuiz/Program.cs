using CyberQuiz.Components;
using CyberQuiz.Components.Account;
using CyberQuiz.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//HTTP resurs som pekar på API.



builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

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
