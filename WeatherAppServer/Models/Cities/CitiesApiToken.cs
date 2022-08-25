using System.Text.Json.Serialization;

namespace WeatherAppServer.Models.Cities
{
    public record CitiesApiToken()
    {
        public static CitiesApiToken Default => new() { Type = nameof(Type), Value = nameof(Value) };

        [JsonPropertyName("token_type")]
        public string Type { get; init; } = nameof(Type);

        [JsonPropertyName("access_token")]
        public string Value { get; init; } = nameof(Value);

        [JsonPropertyName("error_description")]
        public string? Error { get; init; } = "Access token is empty.";

        [JsonIgnore]
        public bool IsFailed
        {
            get => !string.IsNullOrEmpty(Error);
        }
    }
}