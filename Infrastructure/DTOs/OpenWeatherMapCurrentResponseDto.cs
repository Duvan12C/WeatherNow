using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Infrastructure.DTOs
{
    public class OpenWeatherMapCurrentResponseDto
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("data")]
        public List<WeatherBitData> Data { get; set; }
    }

    public class WeatherBitData
    {
        [JsonProperty("city_name")]
        public string CityName { get; set; }

        [JsonProperty("temp")]
        public double Temp { get; set; }

        [JsonProperty("rh")]
        public double Humidity { get; set; }

        [JsonProperty("wind_spd")]
        public double WindSpeed { get; set; }

        [JsonProperty("wind_cdir")]
        public string WindDirection { get; set; }

        [JsonProperty("sunrise")]
        public string Sunrise { get; set; }

        [JsonProperty("sunset")]
        public string Sunset { get; set; }

        [JsonProperty("weather")]
        public WeatherInfo Weather { get; set; }
    }

    public class WeatherInfo
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }
    }

}
