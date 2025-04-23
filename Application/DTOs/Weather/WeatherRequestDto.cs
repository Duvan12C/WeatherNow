using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Weather
{
    public class WeatherRequestDto
    {
        public DateTime FromTime { get; set; }
        public DateTime UntilTime { get; set; }
        public DateTime? AsOf { get; set; } = null;
        public List<CoordinateDto> Coordinates { get; set; } = new();
    }

    public class CoordinateDto
    {
        public string? Name { get; set; }
    }

}
