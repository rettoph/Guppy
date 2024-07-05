using Guppy.Core.Common;
using Microsoft.Xna.Framework;
using System.CommandLine;

namespace Guppy.Game.Common
{
    public interface ITerminal : IConsole
    {
        ITerminalTheme Theme { get; }

        new TextWriter Out { get; }

        IRef<Color> Color { get; set; }

        void Write(string value);

        void Write(string value, IRef<Color> color);

        void WriteLine(string value);

        void WriteLine(string value, IRef<Color> color);

        void NewLine();
    }
}
