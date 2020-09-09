using System;
using System.Threading;
using System.Threading.Tasks;
using Rtl.Assignment.Api.Query;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Rtl.Assignment.Api.Controllers;
using Xunit;

namespace Rtl.Assignment.ApiTests.Controllers
{
    using Api.Abstractions.Response;

    public class ShowWithCastControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;

        private readonly ShowWithCastController _showWithCastController;
        private readonly Fixture _fixture;

        public ShowWithCastControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();

            _showWithCastController = new ShowWithCastController(_mediatorMock.Object, Mock.Of<ILogger<ShowWithCastController>>());

            _fixture = new Fixture();
        }

        [Fact]
        public async Task When_No_Show_Available_Then_Return_NoContent()
        {
            //arrange
            var page = _fixture.Create<int>();
            var token = _fixture.Create<CancellationToken>();
            var emptyShowWithCastResources = 
                _fixture.CreateMany<ShowWithCastResource>(count: 0);

            _mediatorMock.Setup(_ => _.Send(
                It.Is<ShowWithCastQuery>(customer => customer.Page.Equals(page)),
                It.Is<CancellationToken>(providedToken => providedToken == token))).
                ReturnsAsync(emptyShowWithCastResources);

            //act
            var result = await _showWithCastController.GetAllAsync(page, token);

            //assert
            result.Should().BeOfType<NoContentResult>().Which.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task When_Show_Available_ToPresent_Then_Return_Ok()
        {
            //arrange
            var page = _fixture.Create<int>();
            var token = _fixture.Create<CancellationToken>();
            var showWithCastResources =
                _fixture.CreateMany<ShowWithCastResource>();

            _mediatorMock.Setup(_ => _.Send(
                    It.Is<ShowWithCastQuery>(customer => customer.Page.Equals(page)),
                    It.Is<CancellationToken>(providedToken => providedToken == token))).
                ReturnsAsync(showWithCastResources);

            //act
            var result = await _showWithCastController.GetAllAsync(page, token);

            //assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(showWithCastResources);
        }

        [Fact]
        public async Task When_GetAll_Throws_UnhandledException_Then_Return_InternalServerError()
        {
            //arrange
            var page = _fixture.Create<int>();
            var token = _fixture.Create<CancellationToken>();

            _mediatorMock.Setup(_ => _.Send(
                    It.Is<ShowWithCastQuery>(customer => customer.Page.Equals(page)),
                    It.Is<CancellationToken>(providedToken => providedToken == token)))
                .ThrowsAsync(new Exception());

            //act
            var result = await _showWithCastController.GetAllAsync(page, token);

            //assert
            result.Should().BeOfType<StatusCodeResult>().Which.StatusCode.Should().Be(500);
        }
    }
}