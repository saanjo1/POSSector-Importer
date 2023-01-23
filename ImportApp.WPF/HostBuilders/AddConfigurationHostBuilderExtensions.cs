using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ImportApp.WPF.HostBuilders
{
    public static class AddConfigurationHostBuilderExtensions
    {
        public static IHostBuilder AddConfiguration(this IHostBuilder host)
        {
            host.ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("appsettings.json");
                config.AddEnvironmentVariables();
            });

            return host;
        }
    }
}
