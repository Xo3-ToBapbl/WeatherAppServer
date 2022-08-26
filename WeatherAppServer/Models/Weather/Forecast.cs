using System.Text.Json.Serialization;

namespace WeatherAppServer.Models.Weather
{
    public record Forecast
    {
        [JsonPropertyName("maxTemperature")]
        public double MaxTemperature { get; init; }

        [JsonPropertyName("minTemperature")]
        public double MinTemperature { get; init; }

        [JsonPropertyName("wind")]
        public double Wind { get; init; }

        [JsonPropertyName("windDirection")]
        public string? WindDirection { get; init; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; init; }

        [JsonPropertyName("visibility")]
        public double Visibility { get; init; }

        [JsonPropertyName("airPressure")]
        public int AirPressure { get; init; }

        [JsonPropertyName("imageCode")]
        public string? ImageCode { get; init; }

        [JsonPropertyName("date")]
        public string? Date { get; init; }
    }
}
