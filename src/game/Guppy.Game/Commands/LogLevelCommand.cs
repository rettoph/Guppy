﻿using Guppy.Core.Commands.Common;
using Guppy.Core.Commands.Common.Attributes;
using Guppy.Core.Messaging.Common;
using Guppy.Game.Common.Attributes;
using Serilog.Core;
using Serilog.Events;

namespace Guppy.Game
{
    [Command]
    [SceneFilter(null)]
    internal class LogLevelCommand : Message<LogLevelCommand>, ICommand
    {
        public static LoggingLevelSwitch LoggingLevelSwitch = new LoggingLevelSwitch(LogEventLevel.Verbose);

        [Argument]
        public LogEventLevel? Value { get; set; }
    }
}
