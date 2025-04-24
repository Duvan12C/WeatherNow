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

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("current")]
        public async Task<ActionResult<ResponseWeatherApiExternalDto>> GetCurrentWeather([FromQuery] double lat, [FromQuery] double lon)

        {
            try
            {
                var weatherData = await _weatherService.GetCurrentWeatherAsync(lat, lon);
                return Ok(weatherData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al obtener el clima actual.", details = ex.Message });
            }
        }


        [HttpGet("forecast")]
        public async Task<ActionResult<ResponseWeatherForecastDto>> GetWeatherForecastAsync([FromQuery] double lat, [FromQuery] double lon)

        {
            try
            {
                var weatherData = await _weatherService.GetWeatherForecastAsync(lat, lon);
                return Ok(weatherData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al obtener el clima actual.", details = ex.Message });
            }
        }
    }
}
