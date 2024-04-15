using Guppy.Core.Messaging.Common;

namespace Guppy.Game.Commands
{
    public interface ICommandSubscriber<T> : IBaseSubscriber<ICommand, T>
        where T : ICommand
    {
    }
}
