using Guppy.Core.Messaging.Common;

namespace Guppy.Game.Commands.Services
{
    public interface ICommandService : IBroker<ICommand>
    {
        void Invoke(string input);
    }
}
