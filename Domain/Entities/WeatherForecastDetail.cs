using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WeatherForecastDetail
    {
        public DateTime Date { get; set; }
        public string Temperature { get; set; } = string.Empty;
        public string Weather { get; set; } = string.Empty;
    }
}
