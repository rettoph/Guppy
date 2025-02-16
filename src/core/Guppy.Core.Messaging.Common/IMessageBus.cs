using Guppy.Core.Messaging.Common.Utilities;

namespace Guppy.Core.Messaging.Common
{
    public interface IMessageBus
    {
        void Publish<TSequenceGroup, TId, TMessage>(in TId messageId, in TMessage message)
            where TSequenceGroup : unmanaged, Enum;

        void Publish<TSequenceGroup, TMessage>(in TMessage message)
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
            this.Enqueue(OneTimeMessage<TSequenceGroup, TId, TMessage>.Create(in messageId, in message));
        }

        void Enqueue<TSequenceGroup, TMessage>(in TMessage message)
            where TSequenceGroup : unmanaged, Enum
        {
            this.Enqueue(OneTimeMessage<TSequenceGroup, TMessage>.Create(in message));
        }

        void Flush();

        void Subscribe(object subscriber);
        void SubscribeAll(IEnumerable<object> subscribers);
        void Unsubscribe(object subscriber);
        void UnsubscribeAll(IEnumerable<object> subscribers);
    }
}