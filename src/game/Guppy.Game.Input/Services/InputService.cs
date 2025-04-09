using Guppy.Core.Messaging.Common.Services;
using Guppy.Game.Input.Common;
using Guppy.Game.Input.Common.Services;

namespace Guppy.Game.Input.Services
{
    public class InputService(IMessageBusService messageBusService) : IInputService
    {
        private int _inputId = int.MinValue;
        private readonly IMessageBusService _messageBusService = messageBusService;

        public void Publish(IInputMessage input)
        {
            input.Publish(this._inputId++, this._messageBusService);
        }
    }
}