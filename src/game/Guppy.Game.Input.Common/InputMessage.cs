using Guppy.Core.Messaging.Common.Enums;
using Guppy.Core.Messaging.Common.Services;

namespace Guppy.Game.Input.Common
{
    public abstract class InputMessage<TSelf> : IInputMessage
        where TSelf : InputMessage<TSelf>
    {
        private readonly TSelf _casted;

        public InputMessage()
        {
            this._casted = (TSelf)this;
        }

        public void Publish(int inputId, IMessageBusService messageBusService)
        {
            messageBusService.EnqueueAll<SubscriberSequenceGroupEnum, int, TSelf>(inputId, this._casted);
        }
    }
}
