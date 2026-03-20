using QuickFix;

namespace OrderGenerator.Api.Fix.IFix
{
    public interface IFixClient
    {
        void Start();
        void Stop();
        Task<QuickFix.FIX44.ExecutionReport> SendAndAwait(Message message, string clOrdId);
    }
}
