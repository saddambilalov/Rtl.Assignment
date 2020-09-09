namespace Rtl.Assignment.Scraper.Setup
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationSetup
    {
        public static IConfigurationRoot SetupConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            return configuration;
        }
    }
}