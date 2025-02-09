using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Enums;

namespace Guppy.Core.Commands.Common
{
    public class Command<T> : Message<SubscriberSequenceGroupEnum, T>, ICommand
        where T : Command<T>
    {
    }
}
