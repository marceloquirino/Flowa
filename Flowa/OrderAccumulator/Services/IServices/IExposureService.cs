namespace OrderAccumulator.Services.IServices
{
    public interface IExposureService
    {
        decimal UpdateExposure(string symbol, char side, decimal price, decimal quantity);
    }
}
