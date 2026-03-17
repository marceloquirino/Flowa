using OrderAccumulator.Fix.IFix;
using QuickFix;

namespace OrderAccumulator.Fix
{
    public class FixServer : IFixServer
    {
        private readonly ThreadedSocketAcceptor _acceptor;

        public FixServer(FixApplication application)
        {
            var settings = new SessionSettings("fix.cfg");

            IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
            ILogFactory logFactory = new FileLogFactory(settings);
            IMessageFactory messageFactory = new DefaultMessageFactory();

            _acceptor = new ThreadedSocketAcceptor(
                application,
                storeFactory,
                settings,
                logFactory,
                messageFactory
            );
        }

        public void Start()
        {
            _acceptor.Start();
        }

        public void Stop()
        {
            _acceptor.Stop();
        }
    }
}
