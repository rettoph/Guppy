using Guppy.Core.Resources.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Common
{
    public static class GuppyResources
    {
        public static class Colors
        {
            public static readonly ResourceKey<Color> TerminalDefault = ResourceKey<Color>.Get($"{nameof(Color)}.{nameof(TerminalDefault)}", Color.White);
            public static readonly ResourceKey<Color> TerminalFatal = ResourceKey<Color>.Get($"{nameof(Color)}.{nameof(TerminalFatal)}", Color.Red);
            public static readonly ResourceKey<Color> TerminalError = ResourceKey<Color>.Get($"{nameof(Color)}.{nameof(TerminalError)}", Color.Red);
            public static readonly ResourceKey<Color> TerminalWarning = ResourceKey<Color>.Get($"{nameof(Color)}.{nameof(TerminalWarning)}", Color.Yellow);
            public static readonly ResourceKey<Color> TerminalInformation = ResourceKey<Color>.Get($"{nameof(Color)}.{nameof(TerminalInformation)}", Color.LightGray);
            public static readonly ResourceKey<Color> TerminalDebug = ResourceKey<Color>.Get($"{nameof(Color)}.{nameof(TerminalDebug)}", Color.Cyan);
            public static readonly ResourceKey<Color> TerminalVerbose = ResourceKey<Color>.Get($"{nameof(Color)}.{nameof(TerminalVerbose)}", Color.Magenta);
        }
    }
}