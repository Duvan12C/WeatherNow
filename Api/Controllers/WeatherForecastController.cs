using Application.DTOs.Weather;
using Application.Interface;
using Domain.Entities;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(ILogger<WeatherController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpGet("current")]
        public async Task<ActionResult<ResponseWeatherApiExternalDto>> GetCurrentWeather([FromQuery] double lat, [FromQuery] double lon)

        {
            _logger.LogInformation("Recibiendo solicitud GET /weather/current?lat={Lat}&lon={Lon}", lat, lon);

            try
            {
                var weatherData = await _weatherService.GetCurrentWeatherAsync(lat, lon);
                _logger.LogInformation("Datos obtenidos exitosamente para {Lat},{Lon}", lat, lon);
                return Ok(weatherData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener clima actual para {Lat},{Lon}", lat, lon);
                return StatusCode(500, new { message = "Ocurrió un error al obtener el clima actual.", details = ex.Message });
            }
        }


        [HttpGet("forecast")]
        public async Task<ActionResult<ResponseWeatherForecastDto>> GetWeatherForecastAsync([FromQuery] double lat, [FromQuery] double lon)

        {
            _logger.LogInformation("Recibiendo solicitud GET /weather/forecast?lat={Lat}&lon={Lon}&day=7", lat, lon);

            try
            {
                var weatherData = await _weatherService.GetWeatherForecastAsync(lat, lon);
                _logger.LogInformation("Datos obtenidos exitosamente para  forecast 7 dias {Lat},{Lon}", lat, lon);

                return Ok(weatherData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener clima actual para forecast 7 {Lat},{Lon}", lat, lon);
                return StatusCode(500, new { message = "Ocurrió un error al obtener el clima actual.", details = ex.Message });
            }
        }
    }
}
