﻿using Guppy.DependencyInjection;
using Guppy.IO.Services;
using Guppy.IO.Utilities;
using log4net;
using log4net.Core;
using log4net.Layout;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.Extensions.log4net;

namespace Guppy.IO.Extensions.log4net
{
    public static class ILogExtensions
    {
        public static ILog ConfigureTerminalAppender(this ILog log, GuppyServiceProvider provider, PatternLayout layout, params (Level, Color)[] colors)
        {
            var appender = new log4netTerminalAppender(
                terminal: provider.GetService<TerminalService>(),
                layout: layout,
                colors: colors);

            log.AddAppender(appender);

            return log;
        }
        public static ILog ConfigureTerminalAppender(this ILog log, GuppyServiceProvider provider, params (Level, Color)[] colors)
            => log.ConfigureTerminalAppender(provider, new PatternLayout() { ConversionPattern = "[%d{HH:mm:ss}] [%level] %message%n" }, colors);
    }
}
