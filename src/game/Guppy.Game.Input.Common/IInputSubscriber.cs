using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Enums;

namespace Guppy.Game.Input.Common
{
    public interface IInputSubscriber<TInput> : ISubscriber<SubscriberSequenceGroupEnum, int, TInput>
        where TInput : IInputMessage
    {
    }
}