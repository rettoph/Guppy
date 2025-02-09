using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Services;

namespace Guppy.Core.Messaging.Services
{
    public class MessageBusService(Func<MessageBusService, IMessageBus> factory) : IMessageBusService
    {
        private readonly Func<MessageBusService, IMessageBus> _factory = factory;
        private readonly List<IMessageBus> _items = [];

        public IMessageBus Create()
        {
            IMessageBus item = this._factory(this);
            this._items.Add(item);

            return item;
        }

        public void Remove(IMessageBus messageBus)
        {
            this._items.Remove(messageBus);
        }

        public void EnqueueAll<TMessage>(in TMessage message)
            where TMessage : IMessage
        {
            foreach (IMessageBus messageBus in this._items)
            {
                messageBus.Enqueue(message);
            }
        }

        public void EnqueueAll<TSequenceGroup, TId, TMessage>(in TId messageId, in TMessage message) where TSequenceGroup : unmanaged, Enum
        {
            foreach (IMessageBus messageBus in this._items)
            {
                messageBus.Enqueue<TSequenceGroup, TId, TMessage>(messageId, message);
            }
        }

        public void PublishAll<TSequenceGroup, TId, TMessage>(in TId messageId, in TMessage message) where TSequenceGroup : unmanaged, Enum
        {
            foreach (IMessageBus messageBus in this._items)
            {
                messageBus.Publish<TSequenceGroup, TId, TMessage>(messageId, message);
            }
        }

        public void PublishAll<TMessage>(in TMessage message)
            where TMessage : IMessage
        {
            foreach (IMessageBus messageBus in this._items)
            {
                messageBus.Publish(message);
            }
        }

        public IEnumerable<IMessageBus> GetAll()
        {
            return this._items;
        }
    }
}
