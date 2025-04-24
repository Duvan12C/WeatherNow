using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class ResponseWeatherApiExternalDto
    {
        public string CityName { get; set; } 
        public double Temperature { get; set; }
        public string WeatherDescription { get; set; }
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
        public string WindDirection { get; set; }
        public string Sunrise { get; set; }
        public string Sunset { get; set; }
    }

}
