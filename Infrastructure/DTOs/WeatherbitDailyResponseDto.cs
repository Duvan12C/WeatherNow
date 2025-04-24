using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Infrastructure.DTOs
{
    public class WeatherbitDailyResponseDto
    {
        [JsonProperty("data")]
        public List<WeatherbitDailyData> Data { get; set; }

        // Campos opcionales del top-level si se requieren
        [JsonProperty("city_name")]
        public string CityName { get; set; }

        [JsonProperty("lat")]
        public string Lat { get; set; }

        [JsonProperty("lon")]
        public string Lon { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("state_code")]
        public string StateCode { get; set; }
    }


    public class WeatherbitDailyData
    {
        [JsonProperty("valid_date")]
        public string Date { get; set; }    // p.ej. "2025-04-24"
        [JsonProperty("city_name")]
        public string CityName { get; set; }


        [JsonProperty("temp")]
        public double AvgTemp { get; set; }

        [JsonProperty("min_temp")]
        public double MinTemp { get; set; }

        [JsonProperty("max_temp")]
        public double MaxTemp { get; set; }

        [JsonProperty("weather")]
        public WeatherInfo2 Weather { get; set; }
    }

    public class WeatherInfo2
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }

    // DTO simplificado para el frontend
    public class DailyForecastDto
    {
        public DateTime Date { get; set; }
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    // Wrapper de la respuesta del servicio
    public class ResponseWeatherForecastDto
    {
        public string CityName { get; set; }
        public List<DailyForecastDto> Forecasts { get; set; }
    }
}
