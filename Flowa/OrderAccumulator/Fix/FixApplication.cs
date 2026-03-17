using OrderAccumulator.Services.IServices;
using QuickFix;
using QuickFix.FIX44;
using Message = QuickFix.Message;

namespace OrderAccumulator.Fix
{
    public class FixApplication(IExposureService exposureService) : IApplication
    {
        private readonly IExposureService _exposureService = exposureService;

        public void OnCreate(SessionID sessionID) { }

        public void OnLogon(SessionID sessionID)
        {
            Console.WriteLine("FIX session connected");
        }

        public void OnLogout(SessionID sessionID) { }

        public void ToAdmin(Message message, SessionID sessionID) { }

        public void FromAdmin(Message message, SessionID sessionID) { }

        public void ToApp(Message message, SessionID sessionId) { }

        public void FromApp(Message message, SessionID sessionID)
        {
            Console.WriteLine("RAW FIX: " + message);
            if (message is NewOrderSingle order)
            {
                HandleOrder(order);
            }
        }

        private void HandleOrder(NewOrderSingle order)
        {
            var symbol = order.Symbol.getValue();
            var qty = order.OrderQty.getValue();
            var price = order.Price.getValue();
            var side = order.Side.getValue();

            _exposureService.UpdateExposure(symbol, side, price, qty);
        }
    }
}
