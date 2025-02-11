namespace Guppy.Core.Messaging.Common
{
    public abstract class Message<TSequenceGroup, TId, TSelf> : IMessage<TId>
        where TSequenceGroup : unmanaged, Enum
        where TSelf : Message<TSequenceGroup, TId, TSelf>
    {
        private TId? _id;
        public TId Id => this._id ??= this.CalculateId();

        protected abstract TId CalculateId();

        public void Publish(IMessageBus messageBus)
        {
            messageBus.Publish<TSequenceGroup, TId, TSelf>(this.Id, (TSelf)this);
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