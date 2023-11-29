﻿using Guppy.Resources;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Constants
{
    public static class Settings
    {
        public static Setting<bool> IsDebugWindowEnabled = Setting.Get<bool>(nameof(IsDebugWindowEnabled));
        public static Setting<bool> IsTerminalWindowEnabled = Setting.Get<bool>(nameof(IsTerminalWindowEnabled));
        public static Setting<LogEventLevel> LogLevel = Setting.Get<LogEventLevel>(nameof(LogLevel));
    }
}
