namespace Guppy.Core.Messaging.Common
{
    public interface IBus : IBroker<IMessage>
    {
        void Enqueue(in IMessage message);

        void Flush();
    }
}
