namespace Rtl.Assignment.Scraper
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Rtl.Assignment.Scraper.Services;
    using Rtl.Assignment.Scraper.Setup;

    public class Program
    {
        public static async Task Main()
        {
            var source = new CancellationTokenSource();
            var cancellationToken = source.Token;

            var configuration = ConfigurationSetup.SetupConfiguration();

            using var serviceScope = ContainerSetup.ConfigureServices(configuration).CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                await serviceProvider.GetRequiredService<IShowDataWriterService>().UpdateDataInDataStore(
                    cancellationToken);

                logger.LogInformation("Done!!!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                source.Cancel();
            }
        }
    }
}
