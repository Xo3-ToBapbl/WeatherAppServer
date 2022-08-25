using WeatherAppServer.Providers;

namespace WeatherAppServer.Handlers
{
    public class CitiesControllerHandler : DelegatingHandler
    {
        private readonly CitiesTokenProvider _tokenProvider;

        public CitiesControllerHandler(CitiesTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new (_tokenProvider.Token.Type, _tokenProvider.Token.Value);
            return base.SendAsync(request, cancellationToken);
        }
    }
}