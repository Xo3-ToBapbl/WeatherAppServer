using Polly;
using System.Net;
using WeatherAppServer.Exceptions;

namespace WeatherAppServer.Application
{
    public class ApplicationPolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> GetWeatherControllerPolicy(HttpRequestMessage _)
        {
            return Policy
                .Handle<Exception>()
                .OrResult<HttpResponseMessage>(response => response.StatusCode is not HttpStatusCode.OK)
                .WaitAndRetryAsync(new[] { TimeSpan.FromSeconds(2) });
        }
    }
}