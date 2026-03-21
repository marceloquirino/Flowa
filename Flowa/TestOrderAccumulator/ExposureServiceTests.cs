using Microsoft.Extensions.Logging;
using NSubstitute;
using OrderAccumulator.Services;
using QuickFix.Fields;

namespace TestOrderAccumulator
{
    public class ExposureServiceTests
    {
        private readonly ILogger<ExposureService> _logger;
        private readonly ExposureService _service;

        public ExposureServiceTests()
        {
            _logger = Substitute.For<ILogger<ExposureService>>();
            _service = new ExposureService(_logger);
        }

        [Fact]
        public void UpdateExposure_ShouldAddExposure_WhenBuy()
        {
            // Arrange
            const string symbol = "PETR4";
            const decimal price = 10;
            const decimal quantity = 2;

            // Act
            _service.UpdateExposure(symbol, Side.BUY, price, quantity);

            // Assert
            var exposure = _service.GetExposure(symbol);
            Assert.Equal(20, exposure);
        }

        [Fact]
        public void UpdateExposure_ShouldSubtractExposure_WhenSell()
        {
            // Arrange
            const string symbol = "VALE3";

            _service.UpdateExposure(symbol, Side.BUY, 10, 2); // +20

            // Act
            _service.UpdateExposure(symbol, Side.SELL, 5, 2); // -10

            // Assert
            var exposure = _service.GetExposure(symbol);
            Assert.Equal(10, exposure);
        }

        [Fact]
        public void UpdateExposure_ShouldInitializeSymbol_WhenNotExists()
        {
            // Act
            _service.UpdateExposure("VIIA4", Side.BUY, 5, 2);

            // Assert
            var exposure = _service.GetExposure("VIIA4");
            Assert.Equal(10, exposure);
        }

        [Fact]
        public void UpdateExposure_ShouldAccumulateValues()
        {
            // Arrange
            const string symbol = "PETR4";

            _service.UpdateExposure(symbol, Side.BUY, 10, 1); // +10
            _service.UpdateExposure(symbol, Side.BUY, 5, 2);  // +10

            // Assert
            var exposure = _service.GetExposure(symbol);
            Assert.Equal(20, exposure);
        }

        [Fact]
        public void UpdateExposure_ShouldLogInformation_WhenEnabled()
        {
            // Arrange
            _logger.IsEnabled(LogLevel.Information).Returns(true);

            // Act
            _service.UpdateExposure("PETR4", Side.BUY, 10, 2);

            // Assert
            _logger.Received(1).Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Any<object>(),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>()
            );
        }
    }
}