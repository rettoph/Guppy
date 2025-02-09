using Guppy.Core.Messaging.Common;

namespace Guppy.Core.Commands.Common
{
    public interface ICommandSubscriber<TCommand> : ISubscriber<TCommand>
        where TCommand : ICommand
    {
    }
}