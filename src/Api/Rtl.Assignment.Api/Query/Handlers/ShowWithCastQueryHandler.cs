namespace Rtl.Assignment.Api.Query.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Rtl.Assignment.Api.Abstractions.Response;
    using Rtl.Assignment.Domain.Repositories;

    public class ShowWithCastQueryHandler : IRequestHandler<ShowWithCastQuery, IEnumerable<ShowWithCastResource>>
    {
        private readonly IShowWithCastRepository showWithCastRepository;
        private readonly IMapper mapper;

        public ShowWithCastQueryHandler(
            IShowWithCastRepository showWithCastRepository,
            IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.showWithCastRepository = showWithCastRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ShowWithCastResource>> Handle(ShowWithCastQuery request, CancellationToken token)
        {
            var showsWithCast = await this.showWithCastRepository.GetAllAsync(request.Page, token);
            var showsWithCastResources = this.mapper.Map<IEnumerable<ShowWithCastResource>>(showsWithCast).ToList();
            OrderDescByCastBirthday(showsWithCastResources);

            return showsWithCastResources;
        }

        private static void OrderDescByCastBirthday(List<ShowWithCastResource> showsWithCastResources)
        {
            showsWithCastResources.ForEach(s => s.Cast = s.Cast.OrderByDescending(c => c.Birthday).ToList());
        }
    }
}
