using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Weather
{
    public class WeatherRequestDto
    {
        public string Status { get; set; } = string.Empty;
        public List<WeatherDataDto> Data { get; set; } = new List<WeatherDataDto>();
    }

    public class WeatherDataDto
    {
        public string Variable { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
    }
}
