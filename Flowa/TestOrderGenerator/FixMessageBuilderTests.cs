using OrderGenerator.Api.Fix;
using QuickFix.Fields;

namespace TestOrderGenerator
{
    public class FixMessageBuilderTests
    {
        private readonly FixMessageBuilder _builder;

        public FixMessageBuilderTests()
        {
            _builder = new FixMessageBuilder();
        }

        [Fact]
        public void BuildNewOrderSingle_ShouldCreateMessage_WithCorrectFields()
        {
            // Arrange
            const string symbol = "PETR4";
            const char side = Side.BUY;
            const decimal price = 10.5m;
            const int quantity = 100;

            // Act
            var message = _builder.BuildNewOrderSingle(symbol, side, price, quantity);

            // Assert
            Assert.Equal(symbol, message.Symbol.getValue());
            Assert.Equal(side, message.Side.getValue());
            Assert.Equal(price, message.Price.getValue());
            Assert.Equal(quantity, message.OrderQty.getValue());
        }

        [Fact]
        public void BuildNewOrderSingle_ShouldSetLimitOrderType()
        {
            // Act
            var message = _builder.BuildNewOrderSingle("VALE3", Side.SELL, 50m, 200);

            // Assert
            Assert.Equal(OrdType.LIMIT, message.OrdType.getValue());
        }

        [Fact]
        public void BuildNewOrderSingle_ShouldGenerateClOrdID()
        {
            // Act
            var message = _builder.BuildNewOrderSingle("VIIA4", Side.BUY, 5m, 10);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(message.ClOrdID.getValue()));
        }

        [Fact]
        public void BuildNewOrderSingle_ShouldSetHandlingInstruction()
        {
            // Act
            var message = _builder.BuildNewOrderSingle("PETR4", Side.BUY, 10m, 50);

            // Assert
            Assert.Equal('1', message.HandlInst.getValue());
        }

        [Fact]
        public void BuildNewOrderSingle_ShouldSetTransactTime()
        {
            // Act
            var before = DateTime.UtcNow;

            var message = _builder.BuildNewOrderSingle("PETR4", Side.BUY, 10m, 50);

            var after = DateTime.UtcNow;

            var transactTime = message.TransactTime.getValue();

            // Assert
            var tolerance = TimeSpan.FromMilliseconds(1);
            Assert.True(transactTime >= before - tolerance && transactTime <= after + tolerance);
        }
    }
}
