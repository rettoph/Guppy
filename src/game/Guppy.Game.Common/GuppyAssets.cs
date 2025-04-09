using Guppy.Core.Assets.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Common
{
    public static class GuppyAssets
    {
        public static class Colors
        {
            public static readonly AssetKey<Color> TerminalDefault = AssetKey<Color>.Get($"{nameof(Color)}.{nameof(TerminalDefault)}", Color.White);
            public static readonly AssetKey<Color> TerminalFatal = AssetKey<Color>.Get($"{nameof(Color)}.{nameof(TerminalFatal)}", Color.Red);
            public static readonly AssetKey<Color> TerminalError = AssetKey<Color>.Get($"{nameof(Color)}.{nameof(TerminalError)}", Color.Red);
            public static readonly AssetKey<Color> TerminalWarning = AssetKey<Color>.Get($"{nameof(Color)}.{nameof(TerminalWarning)}", Color.Yellow);
            public static readonly AssetKey<Color> TerminalInformation = AssetKey<Color>.Get($"{nameof(Color)}.{nameof(TerminalInformation)}", Color.LightGray);
            public static readonly AssetKey<Color> TerminalDebug = AssetKey<Color>.Get($"{nameof(Color)}.{nameof(TerminalDebug)}", Color.Cyan);
            public static readonly AssetKey<Color> TerminalVerbose = AssetKey<Color>.Get($"{nameof(Color)}.{nameof(TerminalVerbose)}", Color.Magenta);
        }
    }
}