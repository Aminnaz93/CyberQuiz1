using CyberQuiz.API.Services;
using CyberQuiz.BLL.Services;
using CyberQuiz.DAL;
using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;  
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


//Lägg till cors policy.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI", policy =>
    {
        policy.WithOrigins("https://localhost:7047")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});



// Add services to the container.
builder.Services.AddControllers();
// När någon frågar efter IQuizService får de en QuizService tillbaka.
builder.Services.AddScoped<IQuizService, QuizService>();
// När någon frågar efter IUserResultService får de en UserResultService tillbaka.
builder.Services.AddScoped<IUserResultService, UserResultService>();
// AI-service för studieråd
builder.Services.AddScoped<IAIService, AIService>();

// Registrera repositories
builder.Services.AddScoped<CyberQuiz.DAL.Repositories.IUserResultRepository, CyberQuiz.DAL.Repositories.UserResultRepository>();
builder.Services.AddScoped<CyberQuiz.DAL.Repositories.IQuestionRepository, CyberQuiz.DAL.Repositories.QuestionRepository>();
builder.Services.AddScoped<CyberQuiz.DAL.Repositories.ICategoryRepository, CyberQuiz.DAL.Repositories.CategoryRepository>();
builder.Services.AddScoped<CyberQuiz.DAL.Repositories.ISubCategoryRepository, CyberQuiz.DAL.Repositories.SubCategoryRepository>();

// När någon frågar efter IUserService får de en UserService tillbaka.
builder.Services.AddScoped<IUserService, UserService>(); 
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// HttpClient för Ollama
builder.Services.AddHttpClient("Ollama", client =>
{
    var ollamaBaseUrl = builder.Configuration["Ollama:BaseUrl"] ?? "http://localhost:11434";
    client.BaseAddress = new Uri(ollamaBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(60);
});

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


//Databas
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Lägg till Data Protection (krävs för Identity tokens)
builder.Services.AddDataProtection();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>() // kpllar till databasen
    .AddSignInManager() // registrerar SignINManager
    .AddDefaultTokenProviders(); // flr lösenordåterställning

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();


//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddIdentityCore<ApplicationUser>(options =>
//{
//    options.SignIn.RequireConfirmedAccount = true;
//    options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
//})
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddSignInManager()
//    .AddDefaultTokenProviders();

//builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();


// JWT så att API förstår och godkänner tokens från UI
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT-nyckel saknas i appsettings.json");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();


var app = builder.Build();

// Seeda testanvändare i development
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            await UserSeeder.SeedTestUserAsync(services);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding test user.");
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowUI");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
