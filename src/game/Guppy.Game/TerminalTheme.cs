using Guppy.Engine.Common;
using Guppy.Game.Common;
using Microsoft.Xna.Framework;
using Serilog.Events;

namespace Guppy.Game
{
    public class TerminalTheme : ITerminalTheme
    {
        public IRef<Color> Get(LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Verbose:
                    return Resources.Colors.TerminalVerbose;

                case LogEventLevel.Debug:
                    return Resources.Colors.TerminalDebug;

                case LogEventLevel.Information:
                    return Resources.Colors.TerminalInformation;

                case LogEventLevel.Warning:
                    return Resources.Colors.TerminalWarning;

                case LogEventLevel.Error:
                    return Resources.Colors.TerminalError;

                case LogEventLevel.Fatal:
                    return Resources.Colors.TerminalFatal;

                default:
                    return Resources.Colors.TerminalDefault;
            }
        }
    }
}
