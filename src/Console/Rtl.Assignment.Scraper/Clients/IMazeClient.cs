namespace Rtl.Assignment.Scraper.Clients
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Refit;
    using Rtl.Assignment.Scraper.Dtos;

    public interface IMazeClient
    {
        [Get("/shows/{showId}/cast")]
        Task<IEnumerable<CastDto>> FetchCastsAsync(int showId, CancellationToken token);

        [Get("/shows?page={page}")]
        Task<IList<ShowDto>> FetchShowsAsync(int page, CancellationToken token);
    }
}