using System.Text.Json.Serialization;
using WeatherAppServer.Converters;

namespace WeatherAppServer.Models.Weather
{
    public record ForecastData
    {
        [JsonPropertyName("cod")]
        public int? Cod { get; init; }

        [JsonPropertyName("message")]
        [JsonConverter(typeof(ForecastErrorConverter))]
        public string? Error { get; init; }

        [JsonPropertyName("dt")]
        public long UnixDate { get; init; }

        [JsonPropertyName("main")]
        public MainForecastData? Main { get; init; }

        [JsonPropertyName("weather")]
        public Weather[] Weather { get; init; } = { };

        [JsonPropertyName("wind")]
        public Wind? Wind { get; init; }

        [JsonPropertyName("visibility")]
        public int Visibility { get; init; }

        [JsonIgnore]
        public DateTimeOffset DateTimeOffset
        {
            get => DateTimeOffset.FromUnixTimeSeconds(UnixDate);
        }
    }
}
