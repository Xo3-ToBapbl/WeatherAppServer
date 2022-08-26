using System.Text.Json.Serialization;
using WeatherAppServer.Converters;

namespace WeatherAppServer.Models.Weather
{
    public record ForecastResponse
    {
        [JsonPropertyName("cod")]
        public int? Cod { get; init; }

        [JsonPropertyName("message")]
        [JsonConverter(typeof(ForecastErrorConverter))]
        public string? Error { get; init; }

        [JsonPropertyName("cnt")]
        public int Cnt { get; init; }

        [JsonPropertyName("list")]
        public ForecastData[] List { get; init; } = { };

        [JsonIgnore]
        public bool IsSucceess
        {
            get => string.IsNullOrEmpty(Error);
        }
    }
}