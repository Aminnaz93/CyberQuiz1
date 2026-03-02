using CyberQuiz.API.Services;
using CyberQuiz.BLL.Services;
using CyberQuiz.DAL;
using CyberQuiz.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
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
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowUI");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
