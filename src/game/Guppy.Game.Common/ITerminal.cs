using Guppy.Core.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Common
{
    public interface ITerminal
    {
        ITerminalTheme Theme { get; }

        TextWriter Out { get; }

        IRef<Color> Color { get; set; }

        void Write(string value);

        void Write(string value, IRef<Color> color);

        void WriteLine(string value);

        void WriteLine(string value, IRef<Color> color);

        void NewLine();
    }
}