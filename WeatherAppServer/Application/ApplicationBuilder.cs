using WeatherAppServer.Controllers;
using WeatherAppServer.Settings.Options;
using WeatherAppServer.Settings.Secrets;
using WeatherAppServer.Utils;

namespace WeatherAppServer.Application
{
    public class WeatherApplicationBuilder
    {
        public static WeatherApplicationBuilder Create(string[] args)
        {
            return new WeatherApplicationBuilder(args);
        }

        private readonly WebApplicationBuilder _builder;
        private readonly ApplicationOptions _applicationOptions;

        private WeatherApplicationBuilder(string[] args)
        {
            _builder = WebApplication.CreateBuilder(args);
            _applicationOptions = _builder.Configuration.GetSection(nameof(ApplicationOptions)).Get<ApplicationOptions>();
        }

        public WebApplication Build()
        {
            RegisterConfigurations();
            RegisterOptions();
            RegisterSecrets();
            RegisterControllers();
            RegisterHttpClients();

            var application = _builder.Build();
            ConfigureApplication(application);

            return application;
        }

        private void RegisterConfigurations()
        {
            _builder.Configuration.AddSecretsConfiguration(_builder.Environment, _applicationOptions);
        }

        private void RegisterOptions()
        {
            _builder.Services.Configure<WeatherServiceOptions>(_builder.Configuration.GetSection(nameof(WeatherServiceOptions)));
        }

        private void RegisterSecrets()
        {
            _builder.Services.Configure<WeatherServiceSecrets>(_builder.Configuration.GetSection(WeatherServiceSecrets.Key));
        }

        private void RegisterControllers()
        {
            _builder.Services.AddControllers();
        }

        private void RegisterHttpClients()
        {
            var weatherOptions = _builder.Configuration.GetSection(nameof(WeatherServiceOptions)).Get<WeatherServiceOptions>();

            _builder.Services
                .AddHttpClient(nameof(WeatherController), client => client.BaseAddress = new(weatherOptions.Host))
                .AddPolicyHandler(ApplicationPolicies.GetWeatherControllerPolicy);
        }

        private void ConfigureApplication(WebApplication application)
        {
            application.UseHttpLogging();
            application.UseCors(policy => policy.WithOrigins(_applicationOptions.AllowedOrigins));
            application.MapControllers();
        }
    }
}