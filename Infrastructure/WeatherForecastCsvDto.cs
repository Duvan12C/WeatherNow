using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class WeatherForecastCsvDto
    {
        public DateTime ForecastedAt { get; set; }
        public DateTime ForecastedTime { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string? Name { get; set; }
        public double Temperatura { get; set; }
    }

}
