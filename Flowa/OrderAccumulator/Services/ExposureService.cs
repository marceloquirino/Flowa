using OrderAccumulator.Services.IServices;
using QuickFix.Fields;
using System.Collections.Concurrent;

namespace OrderAccumulator.Services
{
    public class ExposureService(ILogger<ExposureService> logger) : IExposureService
    {
        private readonly ILogger<ExposureService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly ConcurrentDictionary<string, decimal> _exposures = [];

        public decimal GetExposure(string symbol)
        {
            return _exposures.TryGetValue(symbol, out var value) ? value : 0;
        }

        public void UpdateExposure(string symbol, char side, decimal price, decimal quantity)
        {
            var value = price * quantity;

            _exposures.AddOrUpdate(
                symbol,
                side == Side.BUY ? value : -value,
                (_, current) => current + (side == Side.BUY ? value : -value)
            );

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Exposure {Symbol}: {Exposure}", symbol, _exposures[symbol]);
            }
        }
    }
}
