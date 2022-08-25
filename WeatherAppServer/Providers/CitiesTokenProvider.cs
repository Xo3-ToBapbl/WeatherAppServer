using Microsoft.Extensions.Options;
using System;
using WeatherAppServer.Models.Cities;
using WeatherAppServer.Settings.Options;
using WeatherAppServer.Settings.Secrets;

namespace WeatherAppServer.Providers
{
    public class CitiesTokenProvider
    {
        private const string GenericErrorMessage = "Can't get access token";
        private const string AccuiringTokenMessage = "Accuering access token for 'city' service...";

        private readonly HttpClient _httpClient;
        private readonly ILogger<CitiesTokenProvider> _logger;
        private readonly CitiesServiceSecrets _secrets;
        private readonly CitiesServiceOptions _options;

        public CitiesApiToken Token { get; private set; }

        public CitiesTokenProvider(
            IHttpClientFactory factory, 
            IOptions<CitiesServiceSecrets> secrets,
            IOptions<CitiesServiceOptions> options,
            ILogger<CitiesTokenProvider> logger)
        {
            _logger = logger;
            _secrets = secrets.Value;
            _options = options.Value;
            _httpClient = factory.CreateClient(nameof(CitiesTokenProvider));

            Token = CitiesApiToken.Default;
        }

        public async Task<CitiesApiToken?> Get()
        {
            var uriBuilder = new UriBuilder(_httpClient.BaseAddress!) { Path = $"{_httpClient.BaseAddress!.PathAndQuery}{_options.AuthorizationPath}" };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uriBuilder.Uri)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    {"grant_type",  _secrets.GrantType },
                    {"client_id",  _secrets.ClientId },
                    {"client_secret",  _secrets.ClientSecret },
                })
            };

            try
            {
                _logger.LogInformation(AccuiringTokenMessage);

                var responseMessage = await _httpClient.SendAsync(requestMessage);
                Token = await responseMessage.Content.ReadFromJsonAsync<CitiesApiToken>() ?? CitiesApiToken.Default;
                if (Token.IsFailed)
                {
                    _logger.LogError(Token.Error);
                }

                return Token;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, GenericErrorMessage);

                return CitiesApiToken.Default with { Error = exception.Message };
            }
        }
    }
}