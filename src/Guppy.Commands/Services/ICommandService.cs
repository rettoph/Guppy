using Guppy.Messaging;

namespace Guppy.Commands.Services
{
    public interface ICommandService : IBroker<ICommand>
    {
        void Invoke(string input);
    }
}
