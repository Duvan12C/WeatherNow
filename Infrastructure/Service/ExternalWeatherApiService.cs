using System.Globalization;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.DTOs;
using Infrastructure.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Infrastructure.Service
{
    public class ExternalWeatherApiService : IExternalWeatherApi
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly string _apiToken;
        private readonly IMemoryCache _memoryCache;
        private readonly WeatherDbContext _context; // Inyección

        private readonly ILogger<ExternalWeatherApiService> _logger;

        public ExternalWeatherApiService(
            IConfiguration configuration, 
            HttpClient httpClient, 
            IMemoryCache memoryCache,
            WeatherDbContext context,
            ILogger<ExternalWeatherApiService> logger)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["WeatherApi:BaseUrl"] ?? throw new ArgumentNullException("WeatherApi:BaseUrl");
            _apiToken = configuration["WeatherApi:ApiToken"] ?? throw new ArgumentNullException("WeatherApi:ApiToken");
            _memoryCache = memoryCache;
            _context = context;
            _logger = logger;

        }


        public async Task<ResponseWeatherApiExternalDto> GetCurrentWeatherAsync(double lat, double lon)
        {
            var cacheKey = $"weather_current_{lat}_{lon}";
            if (_memoryCache.TryGetValue(cacheKey, out ResponseWeatherApiExternalDto cached))
                return cached;
            var latString = lat.ToString(CultureInfo.InvariantCulture);
            var lonString = lon.ToString(CultureInfo.InvariantCulture);

            _logger.LogInformation("Recibiendo solicitud GET /weather/current?lat={Lat}&lon={Lon}", lat, lon , "Dentro del servicio de la capa de infrastructure");

            try
            {
                var url = $"{_apiBaseUrl}current?lat={latString}&lon={lonString}&key={_apiToken}";

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Datos obtenidos exitosamente para para el clima actual");

                    var data = await response.Content.ReadAsStringAsync();

                    var rawWeather = JsonConvert.DeserializeObject<OpenWeatherMapCurrentResponseDto>(data);

                    var result = new ResponseWeatherApiExternalDto
                    {
                        CityName = rawWeather.Data[0].CityName,
                        Temperature = rawWeather.Data[0].Temp,
                        WeatherDescription = rawWeather.Data[0].Weather.Description,
                        Humidity = rawWeather.Data[0].Humidity,
                        WindSpeed = rawWeather.Data[0].WindSpeed,
                        WindDirection = rawWeather.Data[0].WindDirection,
                        Sunrise = rawWeather.Data[0].Sunrise,
                        Sunset = rawWeather.Data[0].Sunset,
                        Icon = rawWeather.Data[0].Weather.Icon

                    };

                    var entity = new WeatherLogs
                    {
                        CityName = result.CityName,
                        Temperature = result.Temperature,
                        Description = result.WeatherDescription,
                        Icon = result.Icon,
                        FechaRegistro = DateTime.UtcNow
                    };

                    _context.WeatherLogs.Add(entity);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Registro de clima guardado correctamente en la base de datos para {CityName} a {Temperature}°C", result.CityName, result.Temperature);


                    _memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(10));

                    return result;
                }
                else
                {
                    _logger.LogError("Error al obtener clima actual para lat={Lat}, lon={Lon}. Estado de la respuesta: {StatusCode}", lat, lon, response.StatusCode);
                    throw new Exception($"Error al obtener el clima: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al obtener el clima actual desde OpenWeatherMap para lat={Lat}, lon={Lon}", lat, lon);
                throw new Exception("Error al obtener el clima actual desde OpenWeatherMap", ex);
            }
        }


        public async Task<ResponseWeatherForecastDto> GetWeatherForecastAsync(double lat, double lon)
        {
            var cacheKey = $"weather_7day_{lat}_{lon}";
            if (_memoryCache.TryGetValue(cacheKey, out ResponseWeatherForecastDto cached))
                return cached;

            var latStr = lat.ToString(CultureInfo.InvariantCulture);
            var lonStr = lon.ToString(CultureInfo.InvariantCulture);

            try
            {
                _logger.LogInformation("Recibiendo solicitud GET /weather/forecast/daily?lat={Lat}&lon={Lon}&days=7", lat, lon);

                var url = $"{_apiBaseUrl}forecast/daily?lat={latStr}&lon={lonStr}&key={_apiToken}&days=7";
                var resp = await _httpClient.GetAsync(url);

                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogError("Error al obtener pronóstico para {Lat},{Lon}. Estado de la respuesta: {StatusCode}", lat, lon, resp.StatusCode);
                    throw new Exception($"Error al obtener pronóstico: {resp.StatusCode}");
                }

                var json = await resp.Content.ReadAsStringAsync();
                var raw = JsonConvert.DeserializeObject<WeatherbitDailyResponseDto>(json);

                var list = raw.Data
                    .Select(d => new DailyForecastDto
                    {
                        Date = DateTime.ParseExact(d.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        MinTemp = d.MinTemp,
                        MaxTemp = d.MaxTemp,
                        Description = d.Weather.Description,
                        Icon = d.Weather.Icon
                    })
                    .ToList();

                var result = new ResponseWeatherForecastDto
                {
                    CityName = raw.CityName,
                    Forecasts = list
                };

                _memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(10));

                _logger.LogInformation("Pronóstico de 7 días para {Lat},{Lon} guardado en caché.", lat, lon);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al obtener el pronóstico de clima para {Lat},{Lon}", lat, lon);
                throw new Exception("Error al obtener el clima pronóstico desde OpenWeatherMap", ex);
            }
        }


    }
}
