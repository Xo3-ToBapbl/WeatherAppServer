using System.Text.Json.Serialization;

namespace WeatherAppServer.Models.Cities
{
    public record Address
    {
        [JsonPropertyName("countryCode")]
        public string? CountryCode { get; init; }

        [JsonPropertyName("stateCode")]
        public string? StateCode { get; init; }
    }
}