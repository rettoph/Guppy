using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Services;

namespace Guppy.Core.Messaging
{
    public class ChannelMessageBus(IMessageBusService messageBusService) : MessageBus(messageBusService)
    {
        private readonly Channel<IMessage> _channel = Channel.CreateUnbounded<IMessage>();

        public override void Enqueue<TMessage>(in TMessage message)
        {
            this._channel.Writer.TryWrite(message);
        }

        protected override bool TryGetEnqueuedMessage([MaybeNullWhen(false)] out IMessage message)
        {
            return this._channel.Reader.TryRead(out message);
        }
    }
}
