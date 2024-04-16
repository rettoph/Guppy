using Guppy.Core.Messaging.Common;

namespace Guppy.Core.Commands.Common.Services
{
    public interface ICommandService : IBroker<ICommand>
    {
        void Invoke(string input);
    }
}
