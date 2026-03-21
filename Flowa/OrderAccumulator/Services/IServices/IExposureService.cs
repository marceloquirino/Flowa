namespace OrderAccumulator.Services.IServices
{
    public interface IExposureService
    {
        void UpdateExposure(string symbol, char side, decimal price, decimal quantity);

        decimal GetExposure(string symbol);
    }
}
