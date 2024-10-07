using Guppy.Core.Resources.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Common
{
    public static class GuppyResources
    {
        public static class Colors
        {
            public static readonly Resource<Color> TerminalDefault = Resource<Color>.Get($"{nameof(Color)}.{nameof(TerminalDefault)}", Color.White);
            public static readonly Resource<Color> TerminalFatal = Resource<Color>.Get($"{nameof(Color)}.{nameof(TerminalFatal)}", Color.Red);
            public static readonly Resource<Color> TerminalError = Resource<Color>.Get($"{nameof(Color)}.{nameof(TerminalError)}", Color.Red);
            public static readonly Resource<Color> TerminalWarning = Resource<Color>.Get($"{nameof(Color)}.{nameof(TerminalWarning)}", Color.Yellow);
            public static readonly Resource<Color> TerminalInformation = Resource<Color>.Get($"{nameof(Color)}.{nameof(TerminalInformation)}", Color.LightGray);
            public static readonly Resource<Color> TerminalDebug = Resource<Color>.Get($"{nameof(Color)}.{nameof(TerminalDebug)}", Color.Cyan);
            public static readonly Resource<Color> TerminalVerbose = Resource<Color>.Get($"{nameof(Color)}.{nameof(TerminalVerbose)}", Color.Magenta);
        }
    }
}
