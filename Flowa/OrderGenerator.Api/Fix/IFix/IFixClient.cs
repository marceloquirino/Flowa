using QuickFix;

namespace OrderGenerator.Api.Fix.IFix
{
    public interface IFixClient
    {
        void Start();
        void Stop();
        bool Send(Message message);
    }
}
