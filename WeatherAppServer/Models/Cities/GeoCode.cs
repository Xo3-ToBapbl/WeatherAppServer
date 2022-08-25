using System.Text.Json.Serialization;

namespace WeatherAppServer.Models.Cities
{
    public record GeoCode
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }
}