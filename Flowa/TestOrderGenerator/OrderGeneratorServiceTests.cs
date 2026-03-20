using NSubstitute;
using OrderGenerator.Api.Fix.IFix;
using OrderGenerator.Api.Models.Dtos;
using OrderGenerator.Api.Services;
using QuickFix.Fields;
using QuickFix.FIX44;
using Side = OrderGenerator.Api.Models.Enums.Side;
using Symbol = OrderGenerator.Api.Models.Enums.Symbol;

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
        public async Task NewOrderSingle_DeveRetornarExposure_QuandoTag9001Existe()
        {
            // Arrange
            string clOrdId = Guid.NewGuid().ToString();
            var order = new OrderDto
            {
                Symbol = Symbol.PETR4,
                Side = Side.Buy,
                Price = 10,
                Quantity = 2
            };

            var message = new NewOrderSingle();
            var executionReport = new ExecutionReport();

            executionReport.SetField(new DecimalField(9001, 123.45m));

            _fixBuilder
                .BuildNewOrderSingle(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<char>(), Arg.Any<decimal>(), Arg.Any<int>())
                .Returns(message);

            _fixClient
                .SendAndAwait(Arg.Any<Message>(), Arg.Any<string>())
                .Returns(executionReport);

            // Act
            var result = await _service.NewOrderSingle(order);

            // Assert
            Assert.Equal(123.45m, result);
        }

        [Fact]
        public async Task NewOrderSingle_DeveRetornarZero_QuandoTag9001NaoExiste()
        {
            // Arrange
            var order = new OrderDto
            {
                Symbol = Symbol.PETR4,
                Side = Side.Buy,
                Price = 10,
                Quantity = 2
            };

            var message = new NewOrderSingle();
            var executionReport = new ExecutionReport();

            _fixBuilder
                .BuildNewOrderSingle(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<char>(), Arg.Any<decimal>(), Arg.Any<int>())
                .Returns(message);

            _fixClient
                .SendAndAwait(Arg.Any<Message>(), Arg.Any<string>())
                .Returns(executionReport);

            // Act
            var result = await _service.NewOrderSingle(order);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task NewOrderSingle_DeveChamarFixBuilderCorretamente()
        {
            // Arrange
            var order = new OrderDto
            {
                Symbol = Symbol.VALE3,
                Side = Side.Sell,
                Price = 50,
                Quantity = 10
            };

            var message = new NewOrderSingle();
            var executionReport = new ExecutionReport();

            _fixBuilder
                .BuildNewOrderSingle(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<char>(), Arg.Any<decimal>(), Arg.Any<int>())
                .Returns(message);

            _fixClient
                .SendAndAwait(Arg.Any<Message>(), Arg.Any<string>())
                .Returns(executionReport);

            // Act
            await _service.NewOrderSingle(order);

            // Assert
            _fixBuilder.Received(1).BuildNewOrderSingle(
                Arg.Any<string>(),
                order.Symbol.ToString(),
                (char)order.Side,
                order.Price,
                order.Quantity
            );
        }

        [Fact]
        public async Task NewOrderSingle_DeveChamarFixClientCorretamente()
        {
            // Arrange
            var order = new OrderDto
            {
                Symbol = Symbol.VALE3,
                Side = Side.Sell,
                Price = 50,
                Quantity = 10
            };

            var message = new NewOrderSingle();
            var executionReport = new ExecutionReport();

            _fixBuilder
                .BuildNewOrderSingle(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<char>(), Arg.Any<decimal>(), Arg.Any<int>())
                .Returns(message);

            _fixClient
                .SendAndAwait(message, Arg.Any<string>())
                .Returns(executionReport);

            // Act
            await _service.NewOrderSingle(order);

            // Assert
            await _fixClient.Received(1)
                .SendAndAwait(message, Arg.Any<string>());
        }
    }
}