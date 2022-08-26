using System.Text.Json.Serialization;

namespace WeatherAppServer.Models.Weather
{
    public record Wind
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }

        [JsonPropertyName("deg")]
        public int Deggree { get; set; }

        [JsonPropertyName("gust")]
        public double Gust { get; set; }
    }
}