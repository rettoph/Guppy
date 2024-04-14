namespace Guppy.Messaging
{
    public interface IBus : IBroker<IMessage>
    {
        void Enqueue(in IMessage message);

        void Flush();
    }
}
