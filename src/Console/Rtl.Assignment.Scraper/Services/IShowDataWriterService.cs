namespace Rtl.Assignment.Scraper.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IShowDataWriterService
    {
        Task UpdateDataInDataStore(CancellationToken cancellationToken);
    }
}