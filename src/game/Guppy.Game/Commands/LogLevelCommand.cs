using Guppy.Core.Commands.Common;
using Guppy.Core.Commands.Common.Attributes;
using Guppy.Core.Logging.Common.Enums;

namespace Guppy.Game
{
    [Command]
    internal class LogLevelCommand : Command<LogLevelCommand>, ICommand
    {
        [Argument]
        public LogLevelEnum? Value { get; set; }
    }
}