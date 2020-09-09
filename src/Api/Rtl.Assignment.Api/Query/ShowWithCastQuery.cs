namespace Rtl.Assignment.Api.Query
{
    using System.Collections.Generic;
    using MediatR;
    using Rtl.Assignment.Api.Abstractions.Response;

    public class ShowWithCastQuery: IRequest<IEnumerable<ShowWithCastResource>>
    {
        public int Page { get; set; }
    }
}
