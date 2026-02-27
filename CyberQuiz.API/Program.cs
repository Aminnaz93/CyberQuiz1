using CyberQuiz.BLL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Lägg till cors policy.


builder.Services.AddControllers();
// När någon frågar efter IQuizService får de en QuizService tillbaka.
builder.Services.AddScoped<IQuizService, QuizService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
