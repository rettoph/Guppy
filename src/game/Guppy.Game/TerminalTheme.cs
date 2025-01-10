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
        private readonly Resource<Color> _verbose = resourceService.Get(GuppyResources.Colors.TerminalVerbose);
        private readonly Resource<Color> _debug = resourceService.Get(GuppyResources.Colors.TerminalDebug);
        private readonly Resource<Color> _information = resourceService.Get(GuppyResources.Colors.TerminalInformation);
        private readonly Resource<Color> _warning = resourceService.Get(GuppyResources.Colors.TerminalWarning);
        private readonly Resource<Color> _error = resourceService.Get(GuppyResources.Colors.TerminalError);
        private readonly Resource<Color> _fatal = resourceService.Get(GuppyResources.Colors.TerminalFatal);
        private readonly Resource<Color> _default = resourceService.Get(GuppyResources.Colors.TerminalDefault);

        public IRef<Color> Get(LogEventLevel level)
        {
            return level switch
            {
                LogEventLevel.Verbose => this._verbose,
                LogEventLevel.Debug => this._debug,
                LogEventLevel.Information => this._information,
                LogEventLevel.Warning => this._warning,
                LogEventLevel.Error => this._error,
                LogEventLevel.Fatal => this._fatal,
                _ => (IRef<Color>)this._default,
            };
        }
    }
}