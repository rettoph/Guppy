namespace Guppy.Core.Messaging
{
    public interface ISubscriber<in TMessage> : IBaseSubscriber<IMessage, TMessage>
        where TMessage : IMessage
    {

    }
}
