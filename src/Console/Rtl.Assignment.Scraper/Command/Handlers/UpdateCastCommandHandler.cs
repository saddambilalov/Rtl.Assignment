namespace Rtl.Assignment.Scraper.Command.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

    public class UpdateCastCommandHandler : INotificationHandler<UpdateCastCommand>
    {
        private readonly IShowWithCastRepository showWithCastRepository;
        private readonly IMazeClient mazeClientService;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public UpdateCastCommandHandler(
            IMapper mapper,
            IShowWithCastRepository showWithCastRepository, 
            IMazeClient mazeClientService,
            ILogger<UpdateCastCommandHandler> logger)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.showWithCastRepository = showWithCastRepository ?? throw new ArgumentNullException(nameof(mapper));
            this.mazeClientService = mazeClientService;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task Handle(UpdateCastCommand notification, CancellationToken cancellationToken)
        {
            try
            {
                var castDtos =
                    await this.mazeClientService
                        .FetchCastsAsync(notification.ShowId, cancellationToken);

                await this.showWithCastRepository.UpdateCastAsync(notification.ShowId, new ShowWithCastEntity
                {
                    Id = notification.ShowId,
                    Cast = this.mapper.Map<IEnumerable<PersonEntity>>(castDtos).OrderByDescending(_ => _.Birthday),
                }, cancellationToken);
            }
            catch (ApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                this.logger.LogWarning($"Not found with showId : {notification.ShowId}");
            }
        }
    }
}