using Google.Api.Gax;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.SecretManager.V1;
using WeatherAppServer.Settings.Options;
using WeatherAppServer.Utils;

namespace WeatherAppServer.Providers
{
    public class SecretConfigurationSource : IConfigurationSource
    {
        private readonly ApplicationOptions _options;

        public SecretConfigurationSource(ApplicationOptions options)
        {
            _options = options;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new SecretConfigurationProvider(_options);
        }
    }

    public class SecretConfigurationProvider : ConfigurationProvider
    {
        private readonly ApplicationOptions _options;
        private readonly SecretManagerServiceClient _client;
        private readonly string _projectId;

        public SecretConfigurationProvider(ApplicationOptions options)
        {
            _options = options;
            _client = SecretManagerServiceClient.Create();
            _projectId = Platform.Instance().ProjectId ?? options.GoogleProjectId;
        }

        public override void Load()
        {
            base.Load();

            Console.WriteLine("Reading secrets from Google.Cloud.SecretManager...\n");

            _client.ListSecrets(new ProjectName(_projectId)).ForEach(secret =>
            {
                var secretId = secret.SecretName.SecretId;
                var secretVersionName = new SecretVersionName(
                    secretId: secretId,
                    projectId: secret.SecretName.ProjectId,
                    secretVersionId: _options.GoogleSecretsVersion);

                var secretVersion = _client.AccessSecretVersion(secretVersionName);
                var stringSecretsResult = secretVersion.Payload.Data.ToStringUtf8();

                SetValueFor(secretId, stringSecretsResult);
            });
        }

        private void SetValueFor(string key, string value)
        {
            const char secretsDelimiter = '\n';
            const char secretsValueDelimiter = ':';

            var secretsDictionary = value
                .Split(new[] { secretsDelimiter }, StringSplitOptions.RemoveEmptyEntries)
                .Select(keyValue => keyValue.Split(secretsValueDelimiter))
                .ToDictionary(keyValue => keyValue[0], keyValue => keyValue[1]);

            secretsDictionary.ForEach(pairs => Set($"{key}{secretsValueDelimiter}{pairs.Key}", pairs.Value));
        }
    }
}
