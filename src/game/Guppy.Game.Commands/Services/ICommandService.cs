using Guppy.Core.Messaging;

namespace Guppy.Game.Commands.Services
{
    public interface ICommandService : IBroker<ICommand>
    {
        void Invoke(string input);
    }
}
