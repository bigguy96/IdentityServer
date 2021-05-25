using System.Text.Json.Serialization;

namespace SampleWeb.Models
{
    public class WeatherForecastViewModel
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("temperatureC")]
        public int TemperatureC { get; set; }

        [JsonPropertyName("temperatureF")]
        public int TemperatureF { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }
    }
}