using Application;
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


        /// <summary>
        /// Obtiene el clima actual para una latitud y longitud espec�ficas.
        /// </summary>
        /// <param name="lat">La latitud de la ubicaci�n para obtener el clima.</param>
        /// <param name="lon">La longitud de la ubicaci�n para obtener el clima.</param>
        /// <returns>Los datos del clima actual, incluyendo temperatura, humedad y m�s.</returns>
        /// <response code="200">Devuelve los datos del clima actual.</response>
        /// <response code="500">Si ocurre un error en el servidor al obtener los datos.</response>
        [HttpGet("current")]
        public async Task<ActionResult<ResponseWeatherApiExternalDto>> GetCurrentWeather([FromQuery] double lat, [FromQuery] double lon)

        {
            _logger.LogInformation("Recibiendo solicitud GET /weather/current?lat={Lat}&lon={Lon}", lat, lon);

            try
            {
                var weatherData = await _weatherService.GetCurrentWeatherAsync(lat, lon);
                var apiResponse = new ApiResponse<ResponseWeatherApiExternalDto>(weatherData);  // Envolvemos la respuesta en ApiResponse

                _logger.LogInformation("Datos obtenidos exitosamente para {Lat},{Lon}", lat, lon);
                return Ok(apiResponse); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener clima actual para {Lat},{Lon}", lat, lon);
                return StatusCode(500, new { message = "Ocurri� un error al obtener el clima actual.", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene el pron�stico del clima para los pr�ximos 7 d�as, basado en la latitud y longitud proporcionadas.
        /// </summary>
        /// <param name="lat">La latitud de la ubicaci�n para el pron�stico del clima.</param>
        /// <param name="lon">La longitud de la ubicaci�n para el pron�stico del clima.</param>
        /// <returns>Los datos del pron�stico del clima a 7 d�as, incluyendo temperaturas m�nimas, m�ximas y descripci�n.</returns>
        /// <response code="200">Devuelve los datos del pron�stico de los pr�ximos 7 d�as.</response>
        /// <response code="500">Si ocurre un error en el servidor al obtener los datos del pron�stico.</response>
        [HttpGet("forecast")]
        public async Task<ActionResult<ResponseWeatherForecastDto>> GetWeatherForecastAsync([FromQuery] double lat, [FromQuery] double lon)

        {
            _logger.LogInformation("Recibiendo solicitud GET /weather/forecast?lat={Lat}&lon={Lon}&day=7", lat, lon);

            try
            {
                var weatherData = await _weatherService.GetWeatherForecastAsync(lat, lon);
                var apiResponse = new ApiResponse<ResponseWeatherForecastDto>(weatherData);  // Envolvemos la respuesta en ApiResponse

                _logger.LogInformation("Datos obtenidos exitosamente para  forecast 7 dias {Lat},{Lon}", lat, lon);

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener clima actual para forecast 7 {Lat},{Lon}", lat, lon);
                return StatusCode(500, new { message = "Ocurri� un error al obtener el clima actual.", details = ex.Message });
            }
        }
    }
}
