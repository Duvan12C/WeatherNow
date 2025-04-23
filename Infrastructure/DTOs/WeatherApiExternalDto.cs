using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class WeatherApiExternalDto
    {
        public DateTime FromTime { get; set; }
        public DateTime UntilTime { get; set; }
        public DateTime? AsOf { get; set; } = null;
        public int? MinHorizon { get; set; } = 0;
        public int? MaxHorizon { get; set; } = 48;
        public List<CoordinateDto> Coordinates { get; set; } = new();
        public List<VariableDto> Variables { get; set; } = new();
    }

    public class CoordinateDto
    {
        public double Lat { get; set; } = 29.7604;
        public double Lon { get; set; } = -95.3698;
        public string? Name { get; set; }
    }

    public class VariableDto
    {
        public string Name { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string? Info { get; set; }
        public string? Alias { get; set; }
    }
}
