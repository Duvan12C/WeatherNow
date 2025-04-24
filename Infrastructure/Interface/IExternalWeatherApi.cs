using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.DTOs;

namespace Infrastructure.Interface
{
    public interface IExternalWeatherApi
    {
        Task<ResponseWeatherApiExternalDto> GetCurrentWeatherAsync(double lat, double lon);
        Task<ResponseWeatherForecastDto> GetWeatherForecastAsync(double lat, double lon);
    }
}
