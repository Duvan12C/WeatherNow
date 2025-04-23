using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Weather;
using Domain.Entities;

namespace Application.Interface
{
    public interface IWeatherService
    {
        Task<WeatherData> GetWeatherWithPayloadAsync(WeatherRequestDto weatherRequestDto);

    }
}
