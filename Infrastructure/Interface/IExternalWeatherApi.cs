using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Interface
{
    public interface IExternalWeatherApi
    {
        Task<WeatherData> GetWeatherWithPayloadAsync();
        Task<WeatherForecast> GetWeatherForecastAsync(string city);
    }
}
