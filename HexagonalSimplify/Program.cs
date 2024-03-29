using Application.Services;
using Domain.Adapters;
using Domain.Services;
using Infra.Adapters;
using Infra.ChatGPT.Adapters;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(c =>
{
    c.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10);
});

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient("GitHub", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://api.github.com/");

    // using Microsoft.Net.Http.Headers;
    // The GitHub API requires two headers.
    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.Accept, "application/vnd.github.v3+json");
    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.UserAgent, "HttpRequestsSample");
});

// Inject dependency
builder.Services.AddTransient<IChatGptAdapter, ChatGptAdapter>();
builder.Services.AddTransient<IChatGptService, ChatGptService>();

builder.Services.AddTransient<ISpeechAdapter, SpeechAdapter>();
builder.Services.AddTransient<ISpeechService, SpeechService>();

builder.Services.AddTransient<IWhisperService, WhisperService>();
builder.Services.AddTransient<IWhisperAdapter, WhisperAdapter>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
