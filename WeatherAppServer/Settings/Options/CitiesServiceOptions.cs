namespace WeatherAppServer.Settings.Options
{
    public record CitiesServiceOptions
    {
        public string Host { get; init; } = string.Empty;

        public string SearchPath { get; init; } = string.Empty;

        public string AuthorizationPath { get; init; } = string.Empty;

        public int MaxResponseItems { get; init; } = 1;
    }
}