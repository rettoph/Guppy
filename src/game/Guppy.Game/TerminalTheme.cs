using Guppy.Core.Common;
using Guppy.Core.Logging.Common.Enums;
using Guppy.Core.Assets.Common;
using Guppy.Core.Assets.Common.Services;
using Guppy.Game.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game
{
    public class TerminalTheme(IAssetService assetService) : ITerminalTheme
    {
        private readonly Asset<Color> _verbose = assetService.Get(GuppyAssets.Colors.TerminalVerbose);
        private readonly Asset<Color> _debug = assetService.Get(GuppyAssets.Colors.TerminalDebug);
        private readonly Asset<Color> _information = assetService.Get(GuppyAssets.Colors.TerminalInformation);
        private readonly Asset<Color> _warning = assetService.Get(GuppyAssets.Colors.TerminalWarning);
        private readonly Asset<Color> _error = assetService.Get(GuppyAssets.Colors.TerminalError);
        private readonly Asset<Color> _fatal = assetService.Get(GuppyAssets.Colors.TerminalFatal);
        private readonly Asset<Color> _default = assetService.Get(GuppyAssets.Colors.TerminalDefault);

        public IRef<Color> Get(LogLevelEnum level)
        {
            return level switch
            {
                LogLevelEnum.Verbose => this._verbose,
                LogLevelEnum.Debug => this._debug,
                LogLevelEnum.Information => this._information,
                LogLevelEnum.Warning => this._warning,
                LogLevelEnum.Error => this._error,
                LogLevelEnum.Fatal => this._fatal,
                _ => (IRef<Color>)this._default,
            };
        }
    }
}