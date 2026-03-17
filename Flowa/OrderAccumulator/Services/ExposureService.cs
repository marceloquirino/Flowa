using OrderAccumulator.Services.IServices;

namespace OrderAccumulator.Services
{
    public class ExposureService : IExposureService
    {
        private readonly Dictionary<string, decimal> _exposures = [];

        public void UpdateExposure(string symbol, char side, decimal price, decimal quantity)
        {
            var value = price * quantity;

            if (!_exposures.ContainsKey(symbol))
                _exposures[symbol] = 0;
            if (side == QuickFix.Fields.Side.BUY)
                _exposures[symbol] += value;
            else
                _exposures[symbol] -= value;

            Console.WriteLine($"Exposure {symbol}: {_exposures[symbol]}");
        }
    }
}
