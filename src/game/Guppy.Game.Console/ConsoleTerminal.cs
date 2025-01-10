using System.CommandLine;
using System.CommandLine.IO;
using Guppy.Core.Common;
using Guppy.Game.Common;
using Guppy.Game.Console.Helpers;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Console
{
    internal class ConsoleTerminal(ITerminalTheme theme) : SystemConsole, ITerminal, IConsole
    {
        TextWriter ITerminal.Out => System.Console.Out;

        public IRef<Color> Color { get; set; } = theme.Get(default!);

        public ITerminalTheme Theme { get; } = theme;

        public void NewLine() => System.Console.WriteLine();

        public void Write(string value) => System.Console.Write(value);

        public void Write(string value, IRef<Color> color)
        {
            using (ConsoleHelper.ApplyForegroundColor(color.Value))
            {
                System.Console.Write(value);
            }
        }

        public void WriteLine(string value) => System.Console.WriteLine(value);

        public void WriteLine(string value, IRef<Color> color)
        {
            using (ConsoleHelper.ApplyForegroundColor(color.Value))
            {
                System.Console.WriteLine(value);
            }
        }
    }
}