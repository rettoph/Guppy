using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Enums;

namespace Guppy.Core.Network.Common
{
    public interface INetIncomingMessageSubscriber<T> : ISubscriber<SubscriberSequenceGroupEnum, INetIncomingMessage<T>>
        where T : notnull
    {
    }
}
