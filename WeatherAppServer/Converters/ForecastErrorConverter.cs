using System.Text.Json;
using System.Text.Json.Serialization;

namespace WeatherAppServer.Converters
{
    public class ForecastErrorConverter : JsonConverter<string>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType is JsonTokenType.Number ? string.Empty : reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) { }
    }
}
