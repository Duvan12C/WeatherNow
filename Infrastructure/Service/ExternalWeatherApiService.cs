using System.Globalization;
using Domain.Entities;
using Infrastructure.DTOs;
using Infrastructure.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
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


        public ExternalWeatherApiService(IConfiguration configuration, HttpClient httpClient, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["WeatherApi:BaseUrl"] ?? throw new ArgumentNullException("WeatherApi:BaseUrl");
            _apiToken = configuration["WeatherApi:ApiToken"] ?? throw new ArgumentNullException("WeatherApi:ApiToken");
            _memoryCache = memoryCache;

        }


        public async Task<ResponseWeatherApiExternalDto> GetCurrentWeatherAsync(double lat, double lon)
        {
            var cacheKey = $"weather_current_{lat}_{lon}";
            var latString = lat.ToString(CultureInfo.InvariantCulture);
            var lonString = lon.ToString(CultureInfo.InvariantCulture);

            if (_memoryCache.TryGetValue(cacheKey, out ResponseWeatherApiExternalDto cachedWeatherData))
            {
                return cachedWeatherData;
            }

            try
            {
                var url = $"{_apiBaseUrl}current?lat={latString}&lon={lonString}&key={_apiToken}";

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
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
                        Sunset = rawWeather.Data[0].Sunset
                    };

                    _memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(10));

                    return result;
                }
                else
                {
                    throw new Exception($"Error al obtener el clima: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
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
            var url = $"{_apiBaseUrl}forecast/daily?lat={latStr}&lon={lonStr}&key={_apiToken}&days=7";

            var resp = await _httpClient.GetAsync(url);
            if (!resp.IsSuccessStatusCode)
                throw new Exception($"Error al obtener pronóstico: {resp.StatusCode}");

            var json = await resp.Content.ReadAsStringAsync();
            // 1) Deserializamos al DTO bruto
            var raw = JsonConvert.DeserializeObject<WeatherbitDailyResponseDto>(json);

            // 2) Mapeamos a nuestro DTO simplificado
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
            return result;
        }

    }
}
