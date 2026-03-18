using NSubstitute;
using OrderGenerator.Api.Fix.IFix;
using OrderGenerator.Api.Models.Dtos;
using OrderGenerator.Api.Models.Enums;
using OrderGenerator.Api.Services;
using QuickFix.FIX44;

namespace TestOrderGenerator
{
    public class OrderGeneratorServiceTests
    {
        private readonly IFixClient _fixClient;
        private readonly IFixMessageBuilder _fixBuilder;
        private readonly OrderGeneratorService _service;

        public OrderGeneratorServiceTests()
        {
            _fixClient = Substitute.For<IFixClient>();
            _fixBuilder = Substitute.For<IFixMessageBuilder>();

            _service = new OrderGeneratorService(_fixClient, _fixBuilder);
        }

        [Fact]
        public void NewOrderSingle_ShouldBuildMessage_AndSendSuccessfully()
        {
            // Arrange
            var order = new OrderDto
            {
                Symbol = Symbol.PETR4,
                Side = Side.Buy,
                Price = 10,
                Quantity = 100
            };

            var expectedMessage = new NewOrderSingle();

            _fixBuilder
                .BuildNewOrderSingle("PETR4", (char)Side.Buy, 10, 100)
                .Returns(expectedMessage);

            _fixClient.Send(expectedMessage).Returns(true);

            // Act
            var result = _service.NewOrderSingle(order);

            // Assert
            Assert.True(result);

            _fixBuilder.Received(1).BuildNewOrderSingle(
                "PETR4",
                (char)Side.Buy,
                10,
                100);

            _fixClient.Received(1).Send(expectedMessage);
        }

        [Fact]
        public void NewOrderSingle_ShouldReturnFalse_WhenSendFails()
        {
            // Arrange
            var order = new OrderDto
            {
                Symbol = Symbol.VALE3,
                Side = Side.Sell,
                Price = 50,
                Quantity = 200
            };

            var message = new NewOrderSingle();

            _fixBuilder
                .BuildNewOrderSingle("VALE3", (char)Side.Sell, 50, 200)
                .Returns(message);

            _fixClient.Send(message).Returns(false);

            // Act
            var result = _service.NewOrderSingle(order);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void NewOrderSingle_ShouldCallDependencies_WithCorrectParameters()
        {
            // Arrange
            var order = new OrderDto
            {
                Symbol = Symbol.VIIA4,
                Side = Side.Buy,
                Price = 5,
                Quantity = 10
            };

            _fixBuilder
                .BuildNewOrderSingle(Arg.Any<string>(), Arg.Any<char>(), Arg.Any<decimal>(), Arg.Any<int>())
                .Returns(new NewOrderSingle());

            _fixClient.Send(Arg.Any<NewOrderSingle>()).Returns(true);

            // Act
            _service.NewOrderSingle(order);

            // Assert
            _fixBuilder.Received().BuildNewOrderSingle(
                "VIIA4",
                (char)Side.Buy,
                5,
                10);
        }
    }
}