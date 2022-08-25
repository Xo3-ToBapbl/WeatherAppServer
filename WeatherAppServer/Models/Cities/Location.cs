using System.Text.Json.Serialization;

namespace WeatherAppServer.Models.Cities
{
    public record class Location
    {
        [JsonPropertyName("type")]
        public string? Type { get; init; }

        [JsonPropertyName("subType")]
        public string? SubType { get; init; }

        [JsonPropertyName("name")]
        public string? Name { get; init; }

        [JsonPropertyName("iataCode")]
        public string? IataCode { get; init; }

        [JsonPropertyName("address")]
        public Address? Address { get; init; }

        [JsonPropertyName("geoCode")]
        public GeoCode? GeoCode { get; init; }
    }
}