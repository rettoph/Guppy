using Guppy.Resources;
using Microsoft.Xna.Framework;

namespace Guppy.Game
{
    public static class Resources
    {
        public static class Colors
        {
            public static readonly Resource<Color> TerminalDefault = Resource.Get<Color>($"{nameof(Color)}.{nameof(TerminalDefault)}");
            public static readonly Resource<Color> TerminalFatal = Resource.Get<Color>($"{nameof(Color)}.{nameof(TerminalFatal)}");
            public static readonly Resource<Color> TerminalError = Resource.Get<Color>($"{nameof(Color)}.{nameof(TerminalError)}");
            public static readonly Resource<Color> TerminalWarning = Resource.Get<Color>($"{nameof(Color)}.{nameof(TerminalWarning)}");
            public static readonly Resource<Color> TerminalInformation = Resource.Get<Color>($"{nameof(Color)}.{nameof(TerminalInformation)}");
            public static readonly Resource<Color> TerminalDebug = Resource.Get<Color>($"{nameof(Color)}.{nameof(TerminalDebug)}");
            public static readonly Resource<Color> TerminalVerbose = Resource.Get<Color>($"{nameof(Color)}.{nameof(TerminalVerbose)}");
        }
    }
}
