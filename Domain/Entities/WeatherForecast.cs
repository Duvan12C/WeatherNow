using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WeatherForecast
    {
        public string City { get; set; } = string.Empty;
        public List<WeatherForecastDetail> ForecastDetails { get; set; } = new List<WeatherForecastDetail>();
    }
}
