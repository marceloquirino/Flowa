using OrderGenerator.Api.Fix.IFix;
using QuickFix;
using QuickFix.Transport;

namespace OrderGenerator.Api.Fix
{
    public class FixClient : IFixClient
    {
        private readonly SocketInitiator _initiator;
        private readonly SessionSettings _settings;
        private readonly FixApplication _application;

        public FixClient()
        {
            _settings = new SessionSettings("fix.cfg");

            _application = new FixApplication();

            IMessageStoreFactory storeFactory = new FileStoreFactory(_settings);
            ILogFactory logFactory = new FileLogFactory(_settings);
            IMessageFactory messageFactory = new DefaultMessageFactory();

            _initiator = new SocketInitiator(
                _application,
                storeFactory,
                _settings,
                logFactory,
                messageFactory
            );
        }

        public bool Send(Message message)
        {
            bool send = false;
            foreach (var session in _initiator.GetSessionIDs())
            {
                send = Session.SendToTarget(message, session);
                if (!send)
                {
                    break;
                }
            }

            return send;
        }

        public void Start()
        {
            _initiator.Start();
        }

        public void Stop()
        {
            _initiator.Stop();
        }
    }
}
