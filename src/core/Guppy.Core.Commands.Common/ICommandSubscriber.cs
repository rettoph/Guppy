using Guppy.Core.Messaging.Common;

namespace Guppy.Core.Commands.Common
{
    public interface ICommandSubscriber<T> : IBaseSubscriber<ICommand, T>
        where T : ICommand
    {
    }
}