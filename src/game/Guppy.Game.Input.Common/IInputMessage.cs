using Guppy.Core.Messaging.Common.Services;

namespace Guppy.Game.Input.Common
{
    public interface IInputMessage
    {
        void Publish(int inputId, IMessageBusService messageBusService);
    }
}