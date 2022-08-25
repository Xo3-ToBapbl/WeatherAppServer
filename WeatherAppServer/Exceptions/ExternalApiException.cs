namespace WeatherAppServer.Exceptions
{
    public class ExternalApiException : Exception
    {
        public ExternalApiException(string message, Exception? exception=null)
            :base(message, exception) { }
    }
}