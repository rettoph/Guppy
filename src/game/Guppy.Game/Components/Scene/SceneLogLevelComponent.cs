﻿using Guppy.Core.Commands.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common;
using Serilog.Events;

namespace Guppy.Game.Components.Scene
{
    internal class EngineLogLevelComponent : IEngineComponent, ICommandSubscriber<LogLevelCommand>
    {
        private readonly ITerminal _terminal;

        private readonly SettingValue<LogEventLevel> _logLevel;

        public EngineLogLevelComponent(ITerminal terminal, ISettingService settings)
        {
            this._terminal = terminal;
            this._logLevel = settings.GetValue(Settings.LogLevel);

            LogLevelCommand.LoggingLevelSwitch.MinimumLevel = this._logLevel;
        }

        [SequenceGroup<InitializeComponentSequenceGroupEnum>(InitializeComponentSequenceGroupEnum.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            //
        }

        public void Process(in Guid messageId, LogLevelCommand message)
        {
            if (message.Value is null)
            {
                this._terminal.Write($"Current Log Level: ");
                this._terminal.WriteLine(LogLevelCommand.LoggingLevelSwitch.MinimumLevel.ToString(), this._terminal.Theme.Get(LogLevelCommand.LoggingLevelSwitch.MinimumLevel));

                return;
            }

            LogLevelCommand.LoggingLevelSwitch.MinimumLevel = this._logLevel.Value = message.Value.Value;

            this._terminal.Write($"Set Log Level: ");
            this._terminal.WriteLine(message.Value.ToString() ?? string.Empty, this._terminal.Theme.Get(message.Value.Value));
        }
    }
}