namespace Rtl.Assignment.Scraper.Services
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Refit;
    using Rtl.Assignment.Domain.Entities;
    using Rtl.Assignment.Domain.Repositories;
    using Rtl.Assignment.Scraper.Clients;
    using Rtl.Assignment.Scraper.Command;
    using Rtl.Assignment.Scraper.Dtos;
    using ILogger = Microsoft.Extensions.Logging.ILogger;

    public class ShowDataWriterService : IShowDataWriterService
    {
        private readonly IMazeClient mazeClientService;
        private readonly IMediator mediator;
        private readonly DataWriterSettings writerSettings;
        private readonly IShowWithCastRepository showWithCastRepository;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public ShowDataWriterService(
            IMazeClient mazeClientService,
            IMediator mediator,
            DataWriterSettings writerSettings,
            ILogger<ShowDataWriterService> logger,
            IShowWithCastRepository showWithCastRepository,
            IMapper mapper)
        {
            this.writerSettings = writerSettings;
            this.mediator = mediator;
            this.logger = logger;
            this.showWithCastRepository = showWithCastRepository;
            this.mapper = mapper;
            this.mazeClientService = mazeClientService;
        }

        public async Task UpdateDataInDataStore(CancellationToken cancellationToken)
        {
            this.logger.LogInformation($"Fetching data from {this.writerSettings.FetchFrom} to {this.writerSettings.FetchTo}");

            var page = this.writerSettings.FetchFrom - 1;
            while (!cancellationToken.IsCancellationRequested
                   || ++page <= this.writerSettings.FetchTo)
            {
                try
                {
                    var showDtos =
                        await this.mazeClientService
                            .FetchShowsAsync(page, cancellationToken);

                    await this.showWithCastRepository.UpdateShowBulkAsync(
                        this.mapper.Map<IEnumerable<ShowWithCastEntity>>(showDtos),
                        cancellationToken);

                    await this.NotifyShowUpdatesAsync(showDtos, cancellationToken);
                }
                catch (ApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
                {
                    this.logger.LogWarning($"Not found with page : {page}");
                    break;
                }
            }
        }

        private async Task NotifyShowUpdatesAsync(
            IEnumerable<ShowDto> showDtos,
            CancellationToken cancellationToken)
        {
            foreach (var showDto in showDtos)
            {
                await this.mediator.Publish(
                    new UpdateCastCommand
                    {
                        ShowId = showDto.Id,
                    }, cancellationToken);
            }
        }
    }
}
