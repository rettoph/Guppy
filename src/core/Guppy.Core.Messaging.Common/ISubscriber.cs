using Guppy.Core.Common.Attributes;

namespace Guppy.Core.Messaging.Common
{
    public interface ISubscriber<TSequenceGroup, TId, TMessage>
        where TSequenceGroup : unmanaged, Enum
    {
        [RequireGenericSequenceGroup(nameof(TSequenceGroup))]
        void Process(in TId id, TMessage message);
    }

    public interface ISubscriber<TSequenceGroup, TMessage>
        where TSequenceGroup : unmanaged, Enum
    {
        [RequireGenericSequenceGroup(nameof(TSequenceGroup))]
        void Process(TMessage message);
    }
}