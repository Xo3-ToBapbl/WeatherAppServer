namespace WeatherAppServer.Settings.Secrets
{
    public record CitiesServiceSecrets
    {
        public const string Key = nameof(CitiesServiceSecrets);

        public string GrantType { get; init; } = string.Empty;

        public string ClientId { get; init; } = string.Empty;

        public string ClientSecret { get; init; } = string.Empty;
    }
}
