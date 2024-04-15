namespace Guppy.Core.Messaging.Common
{
    public interface IBaseSubscriber<TBase>
        where TBase : class, IMessage
    {
    }

    public interface IBaseSubscriber<TBase, in TMessage> : IBaseSubscriber<TBase>
        where TBase : class, IMessage
        where TMessage : TBase
    {
        void Process(in Guid messageId, TMessage message);
    }
}
