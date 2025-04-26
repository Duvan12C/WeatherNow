using System.Text.Json;
using Application.Interface;
using Application.Service;
using Infrastructure.Data;
using Infrastructure.Interface;
using Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Polly;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()  // Puedes agregar m�s sinks, como archivo, etc.
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)  // Log en archivo
    .CreateLogger();

// Usar Serilog en la configuraci�n de logging de ASP.NET Core
builder.Host.UseSerilog();

builder.Services.AddHttpClient("OpenWeatherClient")
    .AddTransientHttpErrorPolicy(policyBuilder =>
        policyBuilder.CircuitBreakerAsync(3, TimeSpan.FromSeconds(30))); // 3 intentos, 30 segundos de pausa si falla


// Configuraci�n de In-Memory Cache
builder.Services.AddMemoryCache();


builder.Services.AddDbContext<WeatherDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
builder.Services.AddScoped<IExternalWeatherApi, ExternalWeatherApiService>();
builder.Services.AddScoped<IWeatherService, WeatherService>(); // Aseg�rate de que WeatherService implementa IWeatherService

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });


// Permitir CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.AllowAnyOrigin()
                      .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

// Usar la pol�tica CORS
app.UseCors("AllowLocalhost");

// Configuraci�n del pipeline HTTP
app.UseSerilogRequestLogging(); // Para log de cada petici�n
app.UseAuthorization();

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
