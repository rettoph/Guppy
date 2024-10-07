using Guppy.Core.Common;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;
using Guppy.Game.Common;
using Microsoft.Xna.Framework;
using Serilog.Events;

namespace Guppy.Game
{
    public class TerminalTheme(IResourceService resourceService) : ITerminalTheme
    {
        private readonly ResourceValue<Color> _verbose = resourceService.GetValue(GuppyResources.Colors.TerminalVerbose);
        private readonly ResourceValue<Color> _debug = resourceService.GetValue(GuppyResources.Colors.TerminalDebug);
        private readonly ResourceValue<Color> _information = resourceService.GetValue(GuppyResources.Colors.TerminalInformation);
        private readonly ResourceValue<Color> _warning = resourceService.GetValue(GuppyResources.Colors.TerminalWarning);
        private readonly ResourceValue<Color> _error = resourceService.GetValue(GuppyResources.Colors.TerminalError);
        private readonly ResourceValue<Color> _fatal = resourceService.GetValue(GuppyResources.Colors.TerminalFatal);
        private readonly ResourceValue<Color> _default = resourceService.GetValue(GuppyResources.Colors.TerminalDefault);

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
