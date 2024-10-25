﻿using Guppy.Core.Common;
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
