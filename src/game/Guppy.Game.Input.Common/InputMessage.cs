using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Enums;

namespace Guppy.Game.Input.Common
{
    public abstract class InputMessage<T> : Message<SubscriberSequenceGroupEnum, int, T>, IInputMessage
        where T : InputMessage<T>
    {
        protected override int CalculateId()
        {
            return this.GetHashCode();
        }
    }
}
