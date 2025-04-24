using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Weather;
using Application.Interface;
using Domain.Entities;
using Infrastructure.DTOs;
using Infrastructure.Interface;

namespace Application.Service
{

    public class WeatherService : IWeatherService
    {
        private readonly IExternalWeatherApi _externalWeatherApi;

        public WeatherService(IExternalWeatherApi externalWeatherApi)
        {
            _externalWeatherApi = externalWeatherApi;
        }

        public async Task<ResponseWeatherApiExternalDto> GetCurrentWeatherAsync(double lat, double lon)
        {
            return await _externalWeatherApi.GetCurrentWeatherAsync(lat, lon);
        }

        public async Task<ResponseWeatherForecastDto> GetWeatherForecastAsync(double lat, double lon)
        {
            return await _externalWeatherApi.GetWeatherForecastAsync(lat, lon);
        }
    }
}
