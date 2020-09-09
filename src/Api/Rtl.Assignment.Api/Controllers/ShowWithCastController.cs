namespace Rtl.Assignment.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Rtl.Assignment.Api.Abstractions.Response;
    using Rtl.Assignment.Api.Query;

    [Route("api/[controller]")]
    [ApiController]
    public class ShowWithCastController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger logger;

        public ShowWithCastController(
            IMediator mediator,
            ILogger<ShowWithCastController> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpGet("{page:int=0}")]
        [ProducesResponseType(typeof(IEnumerable<ShowWithCastResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync(int page, CancellationToken token)
        {
            try
            {
                var showWithCastResources = await this.mediator.Send(
                    new ShowWithCastQuery
                    {
                        Page = page,
                    }, cancellationToken: token);

                if (!showWithCastResources.Any())
                {
                    return this.NoContent();
                }

                return this.Ok(showWithCastResources);
            }
            catch (Exception e)
            {
                this.logger.LogError(e.Message, e);
                return this.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
