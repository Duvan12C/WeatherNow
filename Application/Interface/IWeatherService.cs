using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Weather;
using Domain.Entities;
using Infrastructure.DTOs;

namespace Application.Interface
{
    public interface IWeatherService
    {
        Task<ResponseWeatherApiExternalDto> GetCurrentWeatherAsync(double lat, double lon);
        Task<ResponseWeatherForecastDto> GetWeatherForecastAsync(double lat, double lon);


    }
}
