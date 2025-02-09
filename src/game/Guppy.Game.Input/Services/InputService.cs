using Guppy.Core.Messaging.Common.Services;
using Guppy.Game.Input.Common;
using Guppy.Game.Input.Common.Services;

namespace Guppy.Game.Input.Services
{
    public class InputService(IMessageBusService messageBusService) : IInputService
    {
        private readonly IMessageBusService _messageBusService = messageBusService;

        public void Publish<TInput>(TInput input)
            where TInput : IInput
        {
            this._messageBusService.EnqueueAll(input);
        }
    }
}