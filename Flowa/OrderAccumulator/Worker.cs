using OrderAccumulator.Fix.IFix;

namespace OrderAccumulator
{
    public class Worker(IFixServer fixServer) : BackgroundService
    {
        private readonly IFixServer _fixServer = fixServer;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _fixServer.Start();
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _fixServer.Stop();
            return base.StopAsync(cancellationToken);
        }
    }
}
