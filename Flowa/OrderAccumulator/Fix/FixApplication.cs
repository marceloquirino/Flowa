using OrderAccumulator.Services.IServices;
using QuickFix;
using QuickFix.Fields;
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

        public void ToApp(Message message, SessionID sessionId)
        {
            Console.WriteLine("Message sent: " + message);
        }

        public void FromApp(Message message, SessionID sessionID)
        {
            Console.WriteLine("RAW FIX: " + message);
            if (message is NewOrderSingle order)
            {
                HandleOrder(order, sessionID);
            }
        }

        private void HandleOrder(NewOrderSingle order, SessionID sessionID)
        {
            var symbol = order.Symbol.getValue();
            var qty = order.OrderQty.getValue();
            var price = order.Price.getValue();
            var side = order.Side.getValue();

            var exposure = _exposureService.UpdateExposure(symbol, side, price, qty);

            SendExecutionReport(order, sessionID, exposure);
        }

        private static void SendExecutionReport(NewOrderSingle order, SessionID sessionID, decimal exposure)
        {
            var execReport = new QuickFix.FIX44.ExecutionReport(
                new OrderID(Guid.NewGuid().ToString()),          // 37
                new ExecID(Guid.NewGuid().ToString()),           // 17
                new ExecType(ExecType.FILL),                     // 150
                new OrdStatus(OrdStatus.FILLED),                 // 39
                order.Symbol,                                    // 55
                order.Side,
                new LeavesQty(0),                               // 151
                new CumQty(order.OrderQty.getValue()),           // 14
                new AvgPx(order.Price.getValue())                // 6
            );

            // Campos obrigatórios
            execReport.Set(order.Symbol);                        // 55
            execReport.Set(order.ClOrdID);                       // 11
            execReport.Set(new OrderQty(order.OrderQty.getValue()));
            execReport.Set(new LastQty(order.OrderQty.getValue()));
            execReport.Set(new LastPx(order.Price.getValue()));
            execReport.Set(new TransactTime(DateTime.UtcNow));

            // 💡 Campo customizado para exposure
            execReport.SetField(new DecimalField(9001, exposure));

            Session.SendToTarget(execReport, sessionID);
        }
    }
}
