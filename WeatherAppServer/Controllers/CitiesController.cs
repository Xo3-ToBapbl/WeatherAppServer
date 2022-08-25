using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WeatherAppServer.Models;
using WeatherAppServer.Models.Cities;
using WeatherAppServer.Settings.Options;

namespace WeatherAppServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CitiesController : ControllerBase
    {
        private const string ResponseErrorMessage = "Can't get data for '{0}' keyword.";
        private const string GenericErrorMessage = "Generic exception appeared for '{keyword}' keyword with '{errorMessage}' message";

        private readonly HttpClient _httpClient;
        private readonly ILogger<CitiesController> _logger;
        private readonly CitiesServiceOptions _options;

        private CitiesApiToken _token;

        public CitiesController(
            IOptions<CitiesServiceOptions> options,
            ILogger<CitiesController> logger,
            IHttpClientFactory factory)
        {
            _logger = logger;
            _options = options.Value;
            _token = CitiesApiToken.Default;
            _httpClient = factory.CreateClient(nameof(CitiesController));
        }

        [HttpGet("find/{keyword}")]
        public async Task<GenericResponse<City[]>> FindCitiesBy(string keyword, int max=9)
        {
            var uriBuilder = new UriBuilder(_httpClient.BaseAddress!)
            {
               Path = $"{_httpClient.BaseAddress!.PathAndQuery}{_options.SearchPath}",
               Query = GetSearchQueryString(keyword, max),
            };

            try
            {
                var responseMessage = await _httpClient.SendAsync(new (HttpMethod.Get, uriBuilder.Uri));
                var responseModel = await responseMessage.Content.ReadFromJsonAsync<CitiesResponse>();
                if (responseModel?.IsSucceess is true)
                {
                    return GenericResponse<City[]>.Success(responseModel!.Data!
                        .Where(data => 
                            data.Name is not null && 
                            data.Address?.CountryCode is not null && 
                            data.GeoCode?.Latitude is not null && 
                            data.GeoCode?.Longitude is not null)
                        .Select(data => new City(
                            data.Name!,
                            data.Address!.CountryCode!, 
                            data.GeoCode!.Latitude, 
                            data.GeoCode.Longitude)!)
                        .ToArray());
                }

                responseModel?.Errors?.ToList().ForEach(error => _logger.LogError(error.Detail, error.Code, error.Status));

                return GenericResponse<City[]>.Failed(string.Format(ResponseErrorMessage, keyword));
            }
            catch (Exception exception)
            {
                _logger.LogError(GenericErrorMessage, keyword, exception.Message);
                return GenericResponse<City[]>.Failed(exception.Message);
            }
        }

        public string GetSearchQueryString(string keyword,int max)
        {
            return QueryString.Create(new Dictionary<string, string?>
            {
                { nameof(keyword), keyword },
                { nameof(max), max.ToString() },
            }).Value!;
        }
    }
}