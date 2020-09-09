namespace Rtl.Assignment.Scraper.Command
{
    using MediatR;

    public class UpdateCastCommand : INotification
    {
        public int ShowId { get; set; }
    }
}