using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging.Common.Enums;

namespace Guppy.Core.Messaging.Common
{
    public interface ISubscriber<TSequenceGroup, TId, TMessage>
        where TSequenceGroup : unmanaged, Enum
    {
        [RequireGenericSequenceGroup(nameof(TSequenceGroup))]
        void Process(in TId id, TMessage message);
    }

    public interface ISubscriber<TMessage> : ISubscriber<SubscriberSequenceGroupEnum, int, TMessage>
    {

    }
}