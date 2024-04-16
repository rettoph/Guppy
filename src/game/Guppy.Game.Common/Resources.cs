using Guppy.Core.Resources.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Common
{
    public static class Resources
    {
        public static class Colors
        {
            public static readonly Resource<Color> TerminalDefault = Resource<Color>.Get($"{nameof(Color)}.{nameof(TerminalDefault)}");
            public static readonly Resource<Color> TerminalFatal = Resource<Color>.Get($"{nameof(Color)}.{nameof(TerminalFatal)}");
            public static readonly Resource<Color> TerminalError = Resource<Color>.Get($"{nameof(Color)}.{nameof(TerminalError)}");
            public static readonly Resource<Color> TerminalWarning = Resource<Color>.Get($"{nameof(Color)}.{nameof(TerminalWarning)}");
            public static readonly Resource<Color> TerminalInformation = Resource<Color>.Get($"{nameof(Color)}.{nameof(TerminalInformation)}");
            public static readonly Resource<Color> TerminalDebug = Resource<Color>.Get($"{nameof(Color)}.{nameof(TerminalDebug)}");
            public static readonly Resource<Color> TerminalVerbose = Resource<Color>.Get($"{nameof(Color)}.{nameof(TerminalVerbose)}");
        }
    }
}
