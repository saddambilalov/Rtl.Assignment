namespace Rtl.Assignment.ApiTests.Query.Handlers
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Api.Profiles;
    using Api.Query;
    using Api.Query.Handlers;
    using AutoFixture;
    using AutoMapper;
    using Domain.Entities;
    using Domain.Repositories;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class ShowWithCastQueryHandlerTests
    {
        private readonly ShowWithCastQueryHandler _showWithCastQueryHandler;

        private readonly Mock<IShowWithCastRepository> _showWithCastRepositoryMock;
        private readonly Fixture _fixture;

        public ShowWithCastQueryHandlerTests()
        {
            _showWithCastRepositoryMock = new Mock<IShowWithCastRepository>();
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ShowProfile());
            });

            _showWithCastQueryHandler = new ShowWithCastQueryHandler(
                _showWithCastRepositoryMock.Object,
                mapperConfiguration.CreateMapper());

            _fixture = new Fixture();
        }

        [Fact()]
        public async Task When_Handler_Called_Verify_Repository_Called()
        {
            //arrange
            var request = new ShowWithCastQuery
            {
                Page = _fixture.Create<int>()
            };

            //act
            await _showWithCastQueryHandler.Handle(request, CancellationToken.None);

            //assert
            _showWithCastRepositoryMock.Verify(_ => _.GetAllAsync(
                It.Is<int>(id => id == request.Page),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact()]
        public async Task Verify_Birthday_InDescendingOrder()
        {
            //arrange
            var request = new ShowWithCastQuery
            {
                Page = _fixture.Create<int>()
            };
            var showWithCastEntities = _fixture.CreateMany<ShowWithCastEntity>();

            _showWithCastRepositoryMock
                .Setup(_ => _.GetAllAsync(It.Is<int>(page => page == request.Page),
                CancellationToken.None))
                .ReturnsAsync(showWithCastEntities);

            //act
            var showWithCastResources = await _showWithCastQueryHandler.Handle(request, CancellationToken.None);

            //assert
            _showWithCastRepositoryMock.Verify(_ => _.GetAllAsync(
                It.Is<int>(page => page == request.Page),
                It.IsAny<CancellationToken>()), Times.Once);

            foreach (var showWithCastResource in showWithCastResources)
            {
                showWithCastResource.Cast.Select(_ => _.Birthday).Should().BeInDescendingOrder();
            }
        }
    }
}