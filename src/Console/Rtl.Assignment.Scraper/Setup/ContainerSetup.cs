namespace Rtl.Assignment.Scraper.Setup
{
    using System;
    using System.Reflection;
    using AutoMapper;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Refit;
    using Rtl.Assignment.Infrastructure.DataPersistence.Configuration;
    using Rtl.Assignment.Scraper.Clients;
    using Rtl.Assignment.Scraper.Command;
    using Rtl.Assignment.Scraper.Command.Handlers;
    using Rtl.Assignment.Scraper.Extensions;
    using Rtl.Assignment.Scraper.Profiles;
    using Rtl.Assignment.Scraper.Services;

    public class ContainerSetup
    {
        public static IServiceProvider ConfigureServices(IConfiguration configuration)
        {
            var services = new ServiceCollection();

            services.AddLogging(configure => configure.AddConsole());

            services.Configure<DataWriterSettings>(
                configuration.GetSection(nameof(DataWriterSettings)));
            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<DataWriterSettings>>()?.Value ?? throw new ArgumentNullException(nameof(RtlDatabaseSettings)));

            var clientOptions = configuration.GetSection("Clients:PollyOptions").Get<ClientOptions>();

            services.AddHttpClient("Maze", _ =>
                {
                    _.BaseAddress = configuration.GetValue<Uri>("Clients:Endpoint");
                })
                .AddTypedClient(RestService.For<IMazeClient>)
                .ApplyPolicy(clientOptions);

            services.RegisterMongoDb(configuration);

            services.AddAutoMapper(mapperConfig =>
            {
                mapperConfig.AddProfile<ShowProfile>();
            });

            services.AddSingleton<IShowDataWriterService, ShowDataWriterService>();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddSingleton<INotificationHandler<UpdateCastCommand>, UpdateCastCommandHandler>();

            return services.BuildServiceProvider(validateScopes: true);
        }
    }
}