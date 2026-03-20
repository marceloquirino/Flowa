using OrderAccumulator.Services.IServices;

namespace OrderAccumulator.Services
{
    public class ExposureService(ILogger<ExposureService> logger) : IExposureService
    {
        private readonly ILogger<ExposureService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly Dictionary<string, decimal> _exposures = [];

        public decimal UpdateExposure(string symbol, char side, decimal price, decimal quantity)
        {
            var value = price * quantity;

            if (!_exposures.ContainsKey(symbol))
                _exposures[symbol] = 0;
            if (side == QuickFix.Fields.Side.BUY)
                _exposures[symbol] += value;
            else
                _exposures[symbol] -= value;

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Exposure {Symbol}: {Exposure}", symbol, _exposures[symbol]);
            }

            return _exposures[symbol];
        }
    }
}
