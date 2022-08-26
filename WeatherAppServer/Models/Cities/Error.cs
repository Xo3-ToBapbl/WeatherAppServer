using System.Text.Json.Serialization;

namespace WeatherAppServer.Models.Cities
{
    public record Error(        
        [property: JsonPropertyName("code")] int Code,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("detail")] string Detail,
        [property: JsonPropertyName("status")] int Status);
}
