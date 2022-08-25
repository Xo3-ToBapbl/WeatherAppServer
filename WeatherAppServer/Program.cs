using System.Collections;
using WeatherAppServer.Application;
using WeatherAppServer.Utils;

namespace WeatherAppServer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            DebugVariables(args);

            WeatherApplicationBuilder
                .Create(args)
                .Build()
                .Run();
        }

        private static void DebugVariables(string[] args)
        {
            Console.WriteLine();
#if DEBUG
            Console.WriteLine("Configuration: DEBUG");
            Console.WriteLine();
#elif RELEASE
            Console.WriteLine("Configuration: RELEASE");
            Console.WriteLine();
#else
            Console.WriteLine("Configuration: DEFAULT");
            Console.WriteLine();
#endif

            Console.WriteLine("Environment variables:");
            Environment.GetEnvironmentVariables().ForEach(env => Console.WriteLine($"{((DictionaryEntry)env).Key}:{((DictionaryEntry)env).Value}"));
            Console.WriteLine();
        }
    }
}