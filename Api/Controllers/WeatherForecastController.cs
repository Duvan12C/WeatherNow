using Application.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        // Inyectamos IWeatherService (de la capa Application)
        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("current")]
        public async Task<ActionResult<WeatherData>> GetCurrentWeather()
        {
            try
            {
                var weatherData = await _weatherService.GetWeatherWithPayloadAsync();
                return Ok(weatherData);
            }
            catch (Exception ex)
            {
                // Aquí manejas el error de forma general
                return StatusCode(500, ex.Message);
            }
        }
    }
}
