using QuickFix;

namespace OrderGenerator.Api.Fix
{
    public class FixApplication : IApplication
    {
        public void FromAdmin(Message message, SessionID sessionID) { }

        public void FromApp(Message message, SessionID sessionID)
        {
            Console.WriteLine("Message received: " + message);
        }

        public void OnCreate(SessionID sessionID) { }

        public void OnLogon(SessionID sessionID)
        {
            Console.WriteLine("Logon - FIX session started");
        }

        public void OnLogout(SessionID sessionID) { }

        public void ToAdmin(Message message, SessionID sessionID) { }

        public void ToApp(Message message, SessionID sessionId)
        {
            Console.WriteLine("Message sent: " + message);
        }
    }
}
