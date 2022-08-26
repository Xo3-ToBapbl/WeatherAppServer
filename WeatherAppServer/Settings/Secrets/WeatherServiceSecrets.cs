namespace WeatherAppServer.Settings.Secrets
{
    public record WeatherServiceSecrets
    {
        public const string Key = nameof(WeatherServiceSecrets);

        public string ApiKey { get; init; } = string.Empty;
    }
}
