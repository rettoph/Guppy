namespace Guppy.Core.Messaging.Common
{
    public interface ISubscriber<in TMessage> : IBaseSubscriber<IMessage, TMessage>
        where TMessage : IMessage
    {

    }
}
