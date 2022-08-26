namespace WeatherAppServer.Settings.Options
{
    public record WeatherServiceOptions
    {
        public string Host { get; init; } = string.Empty;

        public string ForecastPath { get; init; } = string.Empty;

        public string CurrentWeatherPath { get; init; } = string.Empty;
    }
}