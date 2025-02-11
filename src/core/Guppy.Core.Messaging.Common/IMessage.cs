namespace Guppy.Core.Messaging.Common
{
    public interface IMessage
    {
        void Publish(IMessageBus messageBus);
    }

    public interface IMessage<TId> : IMessage
    {
        TId Id { get; }
    }
}