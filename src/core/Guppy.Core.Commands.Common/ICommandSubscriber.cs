using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Enums;

namespace Guppy.Core.Commands.Common
{
    public interface ICommandSubscriber<TCommand> : ISubscriber<SubscriberSequenceGroupEnum, TCommand>
        where TCommand : ICommand
    {
    }
}