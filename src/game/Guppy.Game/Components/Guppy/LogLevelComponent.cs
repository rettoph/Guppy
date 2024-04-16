using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Components;
using Guppy.Core.Commands.Common;
using Guppy.Game.Common;

namespace Guppy.Game.Components.Guppy
{
    [AutoLoad]
    internal class LogLevelComponent : GuppyComponent, ICommandSubscriber<LogLevelCommand>
    {
        private readonly ITerminal _terminal;

        public LogLevelComponent(ITerminal terminal)
        {
            _terminal = terminal;

            LogLevelCommand.LoggingLevelSwitch.MinimumLevel = Settings.LogLevel;
        }

        public void Process(in Guid messageId, LogLevelCommand message)
        {
            if (message.Value is null)
            {
                _terminal.Write($"Current Log Level: ");
                _terminal.WriteLine(LogLevelCommand.LoggingLevelSwitch.MinimumLevel.ToString(), _terminal.Theme.Get(LogLevelCommand.LoggingLevelSwitch.MinimumLevel).Value);

                return;
            }

            LogLevelCommand.LoggingLevelSwitch.MinimumLevel = Settings.LogLevel.Value = message.Value.Value;

            _terminal.Write($"Set Log Level: ");
            _terminal.WriteLine(message.Value.Value.ToString(), _terminal.Theme.Get(message.Value.Value).Value);
        }
    }
}
