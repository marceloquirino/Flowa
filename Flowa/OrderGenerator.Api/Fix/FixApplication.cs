using QuickFix;
using System.Collections.Concurrent;

namespace OrderGenerator.Api.Fix
{
    public class FixApplication : MessageCracker, IApplication
    {
        private readonly ConcurrentDictionary<string, TaskCompletionSource<QuickFix.FIX44.ExecutionReport>> _pendingRequests = new();
        public TaskCompletionSource<QuickFix.FIX44.ExecutionReport> ExpectResponse(string clOrdId)
        {
            var tcs = new TaskCompletionSource<QuickFix.FIX44.ExecutionReport>();
            _pendingRequests[clOrdId] = tcs;
            return tcs;
        }
        public void FromAdmin(Message message, SessionID sessionID) { }

        public void FromApp(Message message, SessionID sessionID)
        {
            Console.WriteLine("Message received: " + message);
            // O método crack redireciona a mensagem para o OnMessage correspondente
            if (message is QuickFix.FIX44.ExecutionReport execReport)
            {
                OnMessage(execReport, sessionID);
            }
            else
            {
                Console.WriteLine("Mensagem recebida não é um ExecutionReport conhecido.");
            }
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

        public void OnMessage(QuickFix.FIX44.ExecutionReport message, SessionID sessionID)
        {
            // 1. Lendo campos padrão
            string clOrdID = message.ClOrdID.getValue();
            char ordStatus = message.OrdStatus.getValue();
            decimal lastQty = message.LastQty.getValue();

            Console.WriteLine($"ExecReport recebido! Ordem: {clOrdID}, Status: {ordStatus}, Qtd: {lastQty}");

            // 2. Lendo seu campo customizado (Exposure - Tag 9001)
            if (message.IsSetField(9001))
            {
                decimal exposure = message.GetDecimal(9001);
                Console.WriteLine($"Exposure atualizada via FIX: {exposure}");
            }

            // Se alguém estiver esperando por este ID, completa a Task
            if (_pendingRequests.TryRemove(clOrdID, out var tcs))
            {
                tcs.SetResult(message);
            }
        }
    }
}
