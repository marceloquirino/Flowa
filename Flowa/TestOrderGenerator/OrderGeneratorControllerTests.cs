using NSubstitute;
using OrderGenerator.Api.Controllers;
using OrderGenerator.Api.Models.Dtos;
using OrderGenerator.Api.Services.IService;

namespace TestOrderGenerator
{
    public class OrderGeneratorControllerTests
    {
        private readonly IOrderGeneratorService _service;
        private readonly OrderGeneratorController _controller;

        public OrderGeneratorControllerTests()
        {
            _service = Substitute.For<IOrderGeneratorService>();
            _controller = new OrderGeneratorController(_service);
        }

        [Fact]
        public async Task NewOrderSingle_ShouldReturnSuccess_WhenServiceSucceeds()
        {
            // Arrange
            var order = new OrderDto();

            _service.NewOrderSingle(order).Returns(1);

            // Act
            var result = await _controller.NewOrderSingle(order);

            // Assert
            Assert.True(result.IsSuccess);
            await _service.Received(1).NewOrderSingle(order);
        }

        [Fact]
        public async Task NewOrderSingle_ShouldReturnFailure_WhenServiceThrowsException()
        {
            // Arrange
            var order = new OrderDto();

            _service
                .When(x => x.NewOrderSingle(order))
                .Do(_ => throw new Exception("Erro teste"));

            // Act
            var result = await _controller.NewOrderSingle(order);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Erro teste", result.Message);
        }
    }
}
