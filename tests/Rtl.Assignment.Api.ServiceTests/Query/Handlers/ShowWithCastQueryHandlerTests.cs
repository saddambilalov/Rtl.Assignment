using AutoFixture;
using Moq;
using Rtl.Assignment.Api.Query.Handlers;
using Rtl.Assignment.Domain.Repositories;

namespace Rtl.Assignment.Api.ServiceTests.Query.Handlers
{
    using AutoMapper;

    public class ShowWithCastQueryHandlerTests
    {
        private readonly ShowWithCastQueryHandler _showWithCastQueryHandler;

        private readonly Mock<IShowWithCastRepository> _showWithCastRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly Fixture _fixture;

        public ShowWithCastQueryHandlerTests()
        {
            _showWithCastRepositoryMock = new Mock<IShowWithCastRepository>();
            _mapperMock = new Mock<IMapper>();

            _showWithCastQueryHandler = new ShowWithCastQueryHandler(
                _showWithCastRepositoryMock.Object,
                _mapperMock.Object);

            _fixture = new Fixture();
        }
    }
}