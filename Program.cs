using GeminiAI.Common;
using GeminiAI.Services.Abstract;
using GeminiAI.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);

var appSettings =  Config(builder.Services, builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IGeminiService, GeminiService>();

builder.Services.AddHttpClient("GeminiAI", opt =>
{
    opt.BaseAddress = new Uri($"{appSettings.Url}");
});

AppSettings Config(IServiceCollection services, IConfiguration configuration)
{
    AppSettings appSettings;
    bool isProd = Convert.ToBoolean(Environment.GetEnvironmentVariable("IS_PROD") ?? "false");
    if (isProd)
    {
        appSettings = new AppSettings(isProd);
        services.AddSingleton(appSettings);
        return appSettings;
    }
    appSettings = new AppSettings();
    configuration.GetSection("App").Bind(appSettings);
    services.AddSingleton(appSettings);
    return appSettings;
}


var app = builder.Build();

app.MapGet("/text-only-input", async (IGeminiService geminiService, string prompt) =>
    await geminiService.GetContent(prompt));

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
