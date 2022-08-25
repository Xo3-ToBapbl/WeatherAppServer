namespace WeatherAppServer.Settings.Options
{
    public record ApplicationOptions
    {
        public string[] AllowedOrigins { get; init; } = { };

        public string GoogleProjectId { get; init; } = string.Empty;

        public string GoogleSecretsVersion { get; init; } = string.Empty;
    }
}