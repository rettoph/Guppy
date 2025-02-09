namespace Guppy.Core.Messaging.Common
{
    public interface IMessage
    {
        void Publish(IMessageBus messageBus);
    }
}