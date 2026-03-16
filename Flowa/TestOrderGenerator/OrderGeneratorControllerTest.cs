using NSubstitute;
using OrderGenerator.Api.Controllers;
using OrderGenerator.Api.Models.Dtos;
using OrderGenerator.Api.Services.IService;

namespace TestOrderGenerator
{
    public class OrderGeneratorControllerTest
    {
        private readonly IOrderGeneratorService _service;
        private readonly OrderGeneratorController _controller;

        public OrderGeneratorControllerTest()
        {
            _service = Substitute.For<IOrderGeneratorService>();
            _controller = new OrderGeneratorController(_service);
        }

        [Fact]
        public async Task NewOrderSingle_ShouldReturnSuccess_WhenServiceSucceeds()
        {
            // Arrange
            var order = new OrderDto();

            _service.NewOrderSingle(order).Returns(true);

            // Act
            var result = await _controller.NewOrderSingle(order);

            // Assert
            Assert.True(result.IsSuccess);
            _service.Received(1).NewOrderSingle(order);
        }

        [Fact]
        public async Task NewOrderSingle_ShouldReturnFailure_WhenServiceThrowsException()
        {
            // Arrange
            var order = new OrderDto();

            _service
                .When(x => x.NewOrderSingle(order))
                .Do(x => throw new Exception("Erro teste"));

            // Act
            var result = await _controller.NewOrderSingle(order);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Erro teste", result.Message);
        }
    }
}
