using Guppy.Core.Common.Collections;

namespace Guppy.Core.Messaging.Common.Utilities
{
    /// <summary>
    /// Simple message that can be published once then
    /// will automatically be recycled
    /// </summary>
    /// <typeparam name="TSequenceGroup"></typeparam>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    public class OneTimeMessage<TSequenceGroup, TId, TMessage>() : IMessage
        where TSequenceGroup : unmanaged, Enum
    {
        private static readonly Factory<OneTimeMessage<TSequenceGroup, TId, TMessage>> _factory = new(() => new(), 250);

        private TId _id = default!;
        private TMessage _message = default!;

        public void Publish(IMessageBus messageBus)
        {
            messageBus.Publish<TSequenceGroup, TId, TMessage>(this._id, this._message);
            _factory.TryReturn(this);
        }

        public static OneTimeMessage<TSequenceGroup, TId, TMessage> Create(in TId id, in TMessage message)
        {
            OneTimeMessage<TSequenceGroup, TId, TMessage> instance = _factory.GetOrCreate();
            instance._id = id;
            instance._message = message;

            return instance;
        }
    }

    /// <summary>
    /// Simple message that can be published once then
    /// will automatically be recycled
    /// </summary>
    /// <typeparam name="TSequenceGroup"></typeparam>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    public class OneTimeMessage<TSequenceGroup, TMessage>() : IMessage
        where TSequenceGroup : unmanaged, Enum
    {
        private static readonly Factory<OneTimeMessage<TSequenceGroup, TMessage>> _factory = new(() => new(), 250);

        private TMessage _message = default!;

        public void Publish(IMessageBus messageBus)
        {
            messageBus.Publish<TSequenceGroup, TMessage>(this._message);
            _factory.TryReturn(this);
        }

        public static OneTimeMessage<TSequenceGroup, TMessage> Create(in TMessage message)
        {
            OneTimeMessage<TSequenceGroup, TMessage> instance = _factory.GetOrCreate();
            instance._message = message;

            return instance;
        }
    }
}
