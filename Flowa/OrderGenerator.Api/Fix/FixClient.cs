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
            foreach (var sessionId in _initiator.GetSessionIDs())
            {
                var session = Session.LookupSession(sessionId);

                if (session?.IsLoggedOn != true)
                    return false;

                return Session.SendToTarget(message, sessionId);
            }

            return false;
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
