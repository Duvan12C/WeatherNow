using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WeatherLogs
    {
        public int Id { get; set; } // Este sería el ID de la tabla.
        public string CityName { get; set; }
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
        public string Description { get; set; }
        public double Temperature { get; set; }
        public string Icon { get; set; }
        public DateTime Date { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

}
