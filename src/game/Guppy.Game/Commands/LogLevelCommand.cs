using Guppy.Core.Commands.Common;
using Guppy.Core.Commands.Common.Attributes;
using Guppy.Core.Logging.Common.Enums;
using Guppy.Core.Messaging.Common;

namespace Guppy.Game
{
    [Command]
    internal class LogLevelCommand : Message<LogLevelCommand>, ICommand
    {
        [Argument]
        public LogLevelEnum? Value { get; set; }
    }
}