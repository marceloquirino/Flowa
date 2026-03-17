using OrderGenerator.Api.Fix.IFix;
using QuickFix.Fields;
using QuickFix.FIX44;

namespace OrderGenerator.Api.Fix
{
    public class FixMessageBuilder : IFixMessageBuilder
    {
        public NewOrderSingle BuildNewOrderSingle(
        string symbol,
        char side,
        decimal price,
        int quantity)
        {
            var order = new NewOrderSingle(
                new ClOrdID(Guid.NewGuid().ToString()),
                new Symbol(symbol),
                new Side(side),
                new TransactTime(DateTime.UtcNow),
                new OrdType(OrdType.LIMIT)
            );

            order.Set(new Symbol(symbol));
            order.Set(new OrderQty(quantity));
            order.Set(new Price(price));
            order.Set(new HandlInst('1'));

            return order;
        }
    }
}
