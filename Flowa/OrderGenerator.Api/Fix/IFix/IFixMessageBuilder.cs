using QuickFix.FIX44;

namespace OrderGenerator.Api.Fix.IFix
{
    public interface IFixMessageBuilder
    {
        NewOrderSingle BuildNewOrderSingle(
            string clOrdId,
            string symbol,
            char side,
            decimal price,
            int quantity);
    }
}
