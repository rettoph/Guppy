using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Options
{
    public sealed class GlobalOptions
    {
        /// <summary>
        /// The current provider's game instance.
        /// </summary>
        public Game Game { get; internal set; }

        /// <summary>
        /// The current provider's log level
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.Trace;
    }
}
