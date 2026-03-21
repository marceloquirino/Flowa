using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace OrderGenerator.Api.Services.Telemetry
{
    public static class OrderGeneratorTelemetry
    {
        public const string ActivitySourceName = "OrderGenerator";

        public static readonly ActivitySource ActivitySource =
            new(ActivitySourceName);

        private static readonly Meter Meter =
            new("OrderGeneratorMetrics");

        public static readonly Counter<int> OrdersCreated =
            Meter.CreateCounter<int>("orders_created");

        public static readonly Histogram<double> OrderLatency =
            Meter.CreateHistogram<double>("order_latency_ms");
    }
}
