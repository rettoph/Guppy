using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Services;

namespace Guppy.Core.Messaging
{
    public class ConcurrentQueueMessageBus(IMessageBusService messageBusService) : MessageBus(messageBusService)
    {
        private readonly ConcurrentQueue<IMessage> _queue = [];

        public override void Enqueue<TMessage>(in TMessage message)
        {
            this._queue.Enqueue(message);
        }

        protected override bool TryGetEnqueuedMessage([MaybeNullWhen(false)] out IMessage enqueuedMessage)
        {
            return this._queue.TryDequeue(out enqueuedMessage);
        }
    }
}
