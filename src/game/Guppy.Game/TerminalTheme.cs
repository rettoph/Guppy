using Guppy.Core.Common;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;
using Guppy.Game.Common;
using Microsoft.Xna.Framework;
using Serilog.Events;

namespace Guppy.Game
{
    public class TerminalTheme : ITerminalTheme
    {
        private readonly ResourceValue<Color> _verbose;
        private readonly ResourceValue<Color> _debug;
        private readonly ResourceValue<Color> _information;
        private readonly ResourceValue<Color> _warning;
        private readonly ResourceValue<Color> _error;
        private readonly ResourceValue<Color> _fatal;
        private readonly ResourceValue<Color> _default;

        public TerminalTheme(IResourceService resourceService)
        {
            _verbose = resourceService.GetValue(Resources.Colors.TerminalVerbose);
            _debug = resourceService.GetValue(Resources.Colors.TerminalDebug);
            _information = resourceService.GetValue(Resources.Colors.TerminalInformation);
            _warning = resourceService.GetValue(Resources.Colors.TerminalWarning);
            _error = resourceService.GetValue(Resources.Colors.TerminalError);
            _fatal = resourceService.GetValue(Resources.Colors.TerminalFatal);
            _default = resourceService.GetValue(Resources.Colors.TerminalDefault);
        }

        public IRef<Color> Get(LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Verbose:
                    return _verbose;

                case LogEventLevel.Debug:
                    return _debug;

                case LogEventLevel.Information:
                    return _information;

                case LogEventLevel.Warning:
                    return _warning;

                case LogEventLevel.Error:
                    return _error;

                case LogEventLevel.Fatal:
                    return _fatal;

                default:
                    return _default;
            }
        }
    }
}
