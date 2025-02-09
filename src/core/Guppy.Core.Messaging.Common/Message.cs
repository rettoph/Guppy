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

    public abstract class Message<TSequenceGroup, TSelf> : IMessage
        where TSequenceGroup : unmanaged, Enum
        where TSelf : Message<TSequenceGroup, TSelf>
    {
        public void Publish(IMessageBus messageBus)
        {
            messageBus.Publish<TSequenceGroup, TSelf>((TSelf)this);
        }
    }
}