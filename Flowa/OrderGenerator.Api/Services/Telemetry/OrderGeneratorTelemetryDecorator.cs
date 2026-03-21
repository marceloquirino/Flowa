using OrderGenerator.Api.Models.Dtos;
using OrderGenerator.Api.Services.IService;
using System.Diagnostics;

namespace OrderGenerator.Api.Services.Telemetry
{
    public class OrderGeneratorTelemetryDecorator(IOrderGeneratorService inner) : IOrderGeneratorService
    {
        private readonly IOrderGeneratorService _inner = inner;

        public async Task<decimal> NewOrderSingle(OrderDto order)
        {
            using var activity = OrderGeneratorTelemetry.ActivitySource
                .StartActivity("NewOrderSingle");
            var stopwatch = Stopwatch.StartNew();

            AddTags(activity, order);

            try
            {
                var result = await _inner.NewOrderSingle(order);

                RegisterMetrics(stopwatch, result);

                return result;
            }
            catch (Exception ex)
            {
                RegisterError(activity, ex);
                throw;
            }
        }

        private static void AddTags(Activity? activity, OrderDto order)
        {
            activity?.SetTag("order.symbol", order.Symbol.ToString());
            activity?.SetTag("order.side", order.Side.ToString());
            activity?.SetTag("order.price", order.Price);
            activity?.SetTag("order.quantity", order.Quantity);
            activity?.SetTag("fix.msg_type", "D");
        }

        private static void RegisterMetrics(Stopwatch stopwatch, decimal exposure)
        {
            stopwatch.Stop();

            OrderGeneratorTelemetry.OrdersCreated.Add(1);
            OrderGeneratorTelemetry.OrderLatency.Record(stopwatch.Elapsed.TotalMilliseconds);
        }

        private static void RegisterError(Activity? activity, Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.AddException(ex);
        }
    }
}
