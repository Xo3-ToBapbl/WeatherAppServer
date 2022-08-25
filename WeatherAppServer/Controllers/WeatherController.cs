using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WeatherAppServer.Models;
using WeatherAppServer.Models.Weather;
using WeatherAppServer.Settings.Options;
using WeatherAppServer.Settings.Secrets;
using WeatherAppServer.Utils;

namespace WeatherAppServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private const string ResponseErrorMessage = "Can't get data for longitude '{0}' and longitude '{1}'.";
        private const string GenericErrorMessage = "Generic exception appeared for longitude '{longitude}' and lattitude '{lattitude}' with '{errorMessage}' message";

        private readonly HttpClient _httpClient;
        private readonly ILogger<CitiesController> _logger;
        private readonly WeatherServiceOptions _options;
        private readonly WeatherServiceSecrets _secrets;

        public WeatherController(
            ILogger<CitiesController> logger,
            IOptions<WeatherServiceOptions> options,
            IOptions<WeatherServiceSecrets> secrets,
            IHttpClientFactory factory)
        {
            _logger = logger;
            _options = options.Value;
            _secrets = secrets.Value;
            _httpClient = factory.CreateClient(nameof(WeatherController));
        }

        [HttpGet("forecast")]
        public async Task<GenericResponse<Forecast[]>> GetForecastFor(decimal latitude, decimal longitude)
        {
            var queryString = GetSearchQueryString(latitude, longitude);
            var forecastUriBuilder = new UriBuilder(_httpClient.BaseAddress!)
            {
                Path = $"{_httpClient.BaseAddress!.PathAndQuery}{_options.ForecastPath}",
                Query = GetSearchQueryString(latitude, longitude),
            };
            var currentUriBuilder = new UriBuilder(_httpClient.BaseAddress!)
            {
                Path = $"{_httpClient.BaseAddress!.PathAndQuery}{_options.CurrentWeatherPath}",
                Query = GetSearchQueryString(latitude, longitude),
            };

            try
            {
                var errorMessage = string.Empty;
                var forecastResponseMessage = await _httpClient.SendAsync(new(HttpMethod.Get, forecastUriBuilder.Uri));
                var forecastModel = await forecastResponseMessage.Content.ReadFromJsonAsync<ForecastResponse>();
                if (forecastModel?.IsSucceess is true)
                {
                    var currentResponseMessage = await _httpClient.SendAsync(new(HttpMethod.Get, currentUriBuilder.Uri));
                    var currentModel = await currentResponseMessage.Content.ReadFromJsonAsync<ForecastData>();
                    if (currentResponseMessage.IsSuccessStatusCode)
                    {
                        return GenericResponse<Forecast[]>.Success(result: forecastModel.List
                            .Where(item => item.DateTimeOffset.Hour is 6)
                            .Select(item => item.AsForecast())
                            .Prepend(currentModel!.AsForecast())
                            .ToArray());
                    }

                    errorMessage = currentModel?.Error;
                }
                errorMessage = string.IsNullOrEmpty(forecastModel?.Error) ? ResponseErrorMessage : forecastModel.Error;
                return GenericResponse<Forecast[]>.Failed(string.Format(errorMessage, latitude, longitude));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);

                return GenericResponse<Forecast[]>.Failed(exception.Message);
            }
        }

        public string GetSearchQueryString(decimal latitude, decimal longitude)
        {
            return QueryString.Create(new Dictionary<string, string?>
            {
                { "lat", latitude.ToString() },
                { "lon", longitude.ToString() },
                { "appid", _secrets.ApiKey },
            }).Value!;
        }
    }
}