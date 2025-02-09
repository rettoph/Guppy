using Guppy.Core.Messaging.Common.Enums;

namespace Guppy.Core.Messaging.Common
{
    public abstract class Message<TSequenceGroup, TId, TSelf> : IMessage
        where TSequenceGroup : unmanaged, Enum
        where TSelf : Message<TSequenceGroup, TId, TSelf>
    {
        protected abstract TId GetId();

        public void Publish(IMessageBus messageBus)
        {
            messageBus.Publish<TSequenceGroup, TId, TSelf>(this.GetId(), (TSelf)this);
        }
    }

    public abstract class Message<TSelf> : Message<SubscriberSequenceGroupEnum, int, TSelf>
        where TSelf : Message<TSelf>
    {
        protected override int GetId()
        {
            return this.GetHashCode();
        }
    }
}