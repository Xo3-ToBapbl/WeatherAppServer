using System.Text.Json.Serialization;

namespace WeatherAppServer.Models.Cities
{
    public record CitiesResponse 
    { 
        [JsonPropertyName("data")]
        public Location[] Data { get; init; } = { };

        [JsonPropertyName("errors")]
        public Error[]? Errors { get; init;}

        [JsonIgnore]
        public bool IsSucceess
        {
            get => Errors is null || Errors?.Length is 0;
        }
    }
}