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

        public async Task<QuickFix.FIX44.ExecutionReport> SendAndAwait(Message message, string clOrdId)
        {
            var sessionId = _initiator.GetSessionIDs().FirstOrDefault();
            if (sessionId == null || Session.LookupSession(sessionId)?.IsLoggedOn != true)
                throw new Exception("Fix session not available.");

            // Register and awaits in Application
            var tcs = _application.ExpectResponse(clOrdId);

            // Send the message
            if (!Session.SendToTarget(message, sessionId))
            {
                throw new Exception("Failed to place message in the output queue.");
            }

            // Await response with timeout
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(10));
            var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

            if (completedTask == timeoutTask)
            {
                throw new TimeoutException($"Timeout for message {clOrdId}");
            }

            return await tcs.Task;
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
