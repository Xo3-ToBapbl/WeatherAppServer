using WeatherAppServer.Providers;
using WeatherAppServer.Settings.Options;

namespace WeatherAppServer.Utils
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddSecretsConfiguration(
            this IConfigurationBuilder builder, 
            IWebHostEnvironment environment,
            ApplicationOptions options)
        {
            if (environment.IsProduction())
            {
                builder.Add(new SecretConfigurationSource(options));
            }
            
            return builder;
        }
    }
}