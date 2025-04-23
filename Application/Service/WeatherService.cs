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

        public async Task<WeatherData> GetWeatherWithPayloadAsync(WeatherRequestDto requestDto)
        {
            var ca = new RequestWeatherApiExternalDto
            {
                FromTime = requestDto.FromTime,
                UntilTime = requestDto.UntilTime,
                AsOf = requestDto.AsOf,
                Coordinates = requestDto.Coordinates.Select(c => new Infrastructure.DTOs.CoordinateDto
                {
        
                    Name = c.Name
                }).ToList(),
                Variables = new List<VariableDto>
        {
            new VariableDto
            {
                Name = "TMP",
                Level = "2 m above ground",
                Info = "",
                Alias = "temperatura"
            }
        }

            };
            return await _externalWeatherApi.GetWeatherWithPayloadAsync(ca);
        }
    }
}
