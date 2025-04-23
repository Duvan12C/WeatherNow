using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Domain.Entities;
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

        public async Task<WeatherData> GetWeatherWithPayloadAsync()
        {
            return await _externalWeatherApi.GetWeatherWithPayloadAsync();
        }
    }
}
