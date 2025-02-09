using Guppy.Core.Messaging.Common.Utilities;

namespace Guppy.Core.Messaging.Common
{
    public interface IMessageBus
    {
        void Publish<TSequenceGroup, TId, TMessage>(in TId messageId, in TMessage message)
            where TSequenceGroup : unmanaged, Enum;

        void Publish<TMessage>(in TMessage message)
            where TMessage : IMessage
        {
            message.Publish(this);
        }

        void Enqueue<TMessage>(in TMessage message)
            where TMessage : IMessage;

        void Enqueue<TSequenceGroup, TId, TMessage>(in TId messageId, in TMessage message)
            where TSequenceGroup : unmanaged, Enum
        {
            this.Enqueue(OneTimeMessage<TSequenceGroup, TId, TMessage>.Create(messageId, message));
        }

        void Flush();

        void Subscribe(object subscriber);
        void Subscribe(IEnumerable<object> subscribers);
        void Unsubscribe(object subscriber);
        void Unsubscribe(IEnumerable<object> subscribers);
    }
}