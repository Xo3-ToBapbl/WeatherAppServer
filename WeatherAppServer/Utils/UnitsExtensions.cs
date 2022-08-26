using WeatherAppServer.Models.Weather;

namespace WeatherAppServer.Utils
{
    public static class UnitsExtensions
    {
        static readonly string[] Directions = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW", "N" };

        public static double ToMph(this double metersPerSecond)
        {
            return metersPerSecond * 2.23694;
        }

        public static int ToCelsiues(this double kelvin)
        {
            return (int)Math.Round(kelvin - 273.15, MidpointRounding.AwayFromZero);
        }

        public static string ToDirection(this int degree)
        {
            return Directions[(int)Math.Round((double)degree * 10 % 3600 / 225)];
        }

        public static double ToMiles(this int meters)
        {
            return meters * 0.000621371;
        }

        public static Forecast AsForecast(this ForecastData item)
        {
            return new Forecast
            {
                MaxTemperature = (item.Main?.TempMax ?? 0).ToCelsiues(),
                MinTemperature = (item.Main?.TempMin ?? 0).ToCelsiues(),
                Wind = (item.Wind?.Speed ?? 0).ToMph(),
                WindDirection = (item.Wind?.Deggree ?? 0).ToDirection(),
                Humidity = (item.Main?.Humidity ?? 0),
                AirPressure = (item.Main?.Pressure ?? 0),
                Visibility = item.Visibility.ToMiles(),
                ImageCode = item.Weather?.FirstOrDefault()?.Icon ?? string.Empty,
                Date = item.DateTimeOffset.UtcDateTime.ToShortDateString(),
            };
        } 
    }
}