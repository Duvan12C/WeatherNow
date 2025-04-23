using Domain.Entities;
using Infrastructure.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.Service
{
    public class ExternalWeatherApiService : IExternalWeatherApi
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly string _apiToken;
        private readonly IMemoryCache _memoryCache;


        // Inyectar IConfiguration en el constructor
        public ExternalWeatherApiService(IConfiguration configuration, HttpClient httpClient, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["WeatherApi:BaseUrl"] ?? throw new ArgumentNullException("WeatherApi:BaseUrl");
            _apiToken = configuration["WeatherApi:ApiToken"] ?? throw new ArgumentNullException("WeatherApi:ApiToken");
            _memoryCache = memoryCache;

        }


        public async Task<WeatherData> GetWeatherWithPayloadAsync()
        {
            var cacheKey = "weather_payload_test";

            if (_memoryCache.TryGetValue(cacheKey, out WeatherData cachedWeatherData))
            {
                return cachedWeatherData;
            }

            var payload = new
            {
                fromTime = "2025-03-10T05:00:00Z",
                untilTime = "2025-03-10T10:00:00Z",
                asOf = "2025-03-09T07:30:00Z",
                minHorizon = 0,
                maxHorizon = 48,
                coordinates = new[] {
            new { lat = 29.7604, lon = -95.3698, name = "El Salvador" }
        },
                variables = new[] {
            new { name = "TMP", level = "2 m above ground", info = "", alias = "temperatura" }
        }
            };

            try
            {
                var url = $"{_apiBaseUrl}hrrr/history"; // Asegurate que esta sea la ruta POST correcta

                var jsonContent = JsonConvert.SerializeObject(payload);
                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json")
                };
                request.Headers.Add("Authorization", $"Bearer {_apiToken}");

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonConvert.DeserializeObject<WeatherData>(data);

                    if (weatherData == null)
                    {
                        throw new Exception("Error al deserializar los datos del clima.");
                    }

                    _memoryCache.Set(cacheKey, weatherData, TimeSpan.FromMinutes(10));

                    return weatherData;
                }
                else
                {
                    throw new Exception($"Error al obtener el clima desde payload. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el clima desde payload", ex);
            }
        }



        public async Task<WeatherForecast> GetWeatherForecastAsync(string city)
        {
            var url = $"{_apiBaseUrl}weather/forecast?city={city}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", $"Bearer {_apiToken}");

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();

                try
                {
                    // Intentar deserializar el JSON
                    var weatherForecast = JsonConvert.DeserializeObject<WeatherForecast>(data);

                    if (weatherForecast == null)
                    {
                        throw new Exception("Deserialización de los datos de pronóstico falló. Los datos recibidos son inválidos.");
                    }

                    return weatherForecast;
                }
                catch (JsonException ex)
                {
                    // Manejo de errores en caso de que no se pueda deserializar
                    throw new Exception("Error al deserializar los datos del pronóstico", ex);
                }
            }

            throw new Exception("Error al obtener el pronóstico");
        }
    }
}
