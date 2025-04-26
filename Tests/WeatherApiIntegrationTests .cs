using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class WeatherApiTests : IClassFixture<WebApplicationFactory<Program>>  // Cambié Startup por Program
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public WeatherApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_CurrentWeather_ReturnsOk()
        {
            // Arrange
            var lat = 40.7128;  // Ejemplo de latitud (Nueva York)
            var lon = -74.0060; // Ejemplo de longitud (Nueva York)
            var url = $"/weather/current?lat={lat}&lon={lon}";  // Incluir los parámetros en la URL

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();  // Asegura que el código de estado sea 200 (OK)
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("CityName", responseContent);  // Ejemplo de verificación de contenido
        }
    }
}
