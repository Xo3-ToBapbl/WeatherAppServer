using Polly;
using System.Net;
using WeatherAppServer.Exceptions;
using WeatherAppServer.Providers;

namespace WeatherAppServer.Application
{
    public class ApplicationPolicies
    {
        private static HttpStatusCode[] PolicyStatusCodes =
        {
            HttpStatusCode.Unauthorized,
            HttpStatusCode.Forbidden,
        };

        public static IAsyncPolicy<HttpResponseMessage> GetCitiesControllerPolicy(IServiceProvider provider, HttpRequestMessage _)
        {
            return Policy
                .HandleResult<HttpResponseMessage>(response => PolicyStatusCodes.Contains(response.StatusCode))
                .RetryAsync(2, (_, retryCount, _) =>
                {
                    if (retryCount is 2)
                    {
                        throw new ExternalApiException("Can't get access token for cities 3rd party service.");
                    }
                    return provider.GetService<CitiesTokenProvider>()!.Get();
                });
        }

        public static IAsyncPolicy<HttpResponseMessage> GetWeatherControllerPolicy(HttpRequestMessage _)
        {
            return Policy
                .Handle<Exception>()
                .OrResult<HttpResponseMessage>(response => response.StatusCode is not HttpStatusCode.OK)
                .WaitAndRetryAsync(new[] { TimeSpan.FromSeconds(2) });
        }

        public static IAsyncPolicy<HttpResponseMessage> GetCitiesTokenProviderPolicy(HttpRequestMessage _)
        {
            return Policy
                .Handle<Exception>()
                .OrResult<HttpResponseMessage>(response => response.StatusCode is not HttpStatusCode.OK)
                .WaitAndRetryAsync(new[] { TimeSpan.FromSeconds(2) });
        }
    }
}