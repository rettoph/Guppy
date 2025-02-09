using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Services;
using Guppy.Core.Messaging.Utilities;

namespace Guppy.Core.Messaging
{
    public abstract class MessageBus(MessageBusService messageBusService) : IMessageBus, IDisposable
    {
        private readonly MessageBusService _messageBusService = messageBusService;
        private readonly HashSet<object> _subscribers = [];
        private readonly Dictionary<MessagePublisherKey, IMessagePublisher> _publishers = [];
        private bool _disposed;

        public abstract void Enqueue<TMessage>(in TMessage message)
            where TMessage : IMessage;

        protected abstract bool TryGetEnqueuedMessage([MaybeNullWhen(false)] out IMessage message);

        public void Publish<TSequenceGroup, TId, TMessage>(in TId messageId, in TMessage message)
            where TSequenceGroup : unmanaged, Enum
        {
            this.GetPublisher<TSequenceGroup, TId, TMessage>().Publish(in messageId, in message);
        }

        public void Flush()
        {
            while (this.TryGetEnqueuedMessage(out IMessage? message) == true)
            {
                message.Publish(this);
            }
        }

        public void Subscribe(object subscriber)
        {
            if (this._subscribers.Add(subscriber) == false)
            {
                return;
            }

            foreach (IMessagePublisher publisher in this._publishers.Values)
            {
                publisher.Subscribe(subscriber);
            }
        }

        public void Subscribe(IEnumerable<object> subscribers)
        {
            List<object> incoming = [];
            foreach (object subscriber in subscribers)
            {
                if (this._subscribers.Add(subscriber) == true)
                {
                    incoming.Add(subscriber);
                }
            }

            foreach (IMessagePublisher publisher in this._publishers.Values)
            {
                publisher.Subscribe(incoming);
            }
        }

        public void Unsubscribe(object subscriber)
        {
            if (this._subscribers.Remove(subscriber) == false)
            {
                return;
            }

            foreach (IMessagePublisher publisher in this._publishers.Values)
            {
                publisher.Unsubscribe(subscriber);
            }
        }

        public void Unsubscribe(IEnumerable<object> subscribers)
        {
            List<object> outgoing = [];
            foreach (object subscriber in subscribers)
            {
                if (this._subscribers.Remove(subscriber) == true)
                {
                    outgoing.Add(subscriber);
                }
            }

            foreach (IMessagePublisher publisher in this._publishers.Values)
            {
                publisher.Unsubscribe(outgoing);
            }
        }

        private MessagePublisher<TSequenceGroup, TId, TMessage> GetPublisher<TSequenceGroup, TId, TMessage>()
            where TSequenceGroup : unmanaged, Enum
        {
            ref IMessagePublisher? publisher = ref CollectionsMarshal.GetValueRefOrAddDefault(this._publishers, MessagePublisherKey.Instance<TSequenceGroup, TId, TMessage>.Value, out bool exists);
            if (exists == true)
            {
                return (MessagePublisher<TSequenceGroup, TId, TMessage>)publisher!;
            }

            Type publisherType = typeof(MessagePublisher<,,>).MakeGenericType(typeof(TSequenceGroup), typeof(TId), typeof(TMessage));

            MessagePublisher<TSequenceGroup, TId, TMessage> instance = (MessagePublisher<TSequenceGroup, TId, TMessage>)(Activator.CreateInstance(publisherType) ?? throw new NotImplementedException());
            instance.Subscribe(this._subscribers);

            publisher = instance;
            return instance!;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    this._messageBusService.Remove(this);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                this._disposed = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MessageBus()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}