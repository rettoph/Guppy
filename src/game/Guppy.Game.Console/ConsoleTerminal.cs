using Guppy.Game.Common;
using Guppy.Game.Console.Helpers;
using Microsoft.Xna.Framework;
using System.CommandLine.IO;

namespace Guppy.Game.Console
{
    internal class ConsoleTerminal : SystemConsole, ITerminal
    {
        TextWriter ITerminal.Out => System.Console.Out;

        public Color Color { get; set; }


        public ConsoleTerminal(ITerminalTheme theme)
        {
            this.Theme = theme;
        }

        public ITerminalTheme Theme { get; }

        public void NewLine()
        {
            System.Console.WriteLine();
        }

        public void Write(string value)
        {
            System.Console.Write(value);
        }

        public void Write(string value, Color color)
        {
            using (ConsoleHelper.ApplyForegroundColor(color))
            {
                System.Console.Write(value);
            }
        }

        public void WriteLine(string value)
        {
            System.Console.WriteLine(value);
        }

        public void WriteLine(string value, Color color)
        {
            using (ConsoleHelper.ApplyForegroundColor(color))
            {
                System.Console.WriteLine(value);
            }
        }
    }
}
