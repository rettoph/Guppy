using Guppy.Game.Common;
using Microsoft.Xna.Framework;
using System.CommandLine.IO;

namespace Guppy.Game
{
    internal class ConsoleTerminal : SystemConsole, ITerminal
    {
        TextWriter ITerminal.Out => Console.Out;

        public Color Color { get; set; }

        public ConsoleTerminal(ITerminalTheme theme)
        {
            this.Theme = theme;
        }

        public ITerminalTheme Theme { get; }

        public void NewLine()
        {
            Console.WriteLine();
        }

        public void Write(string value)
        {
            Console.Write(value);
        }

        public void Write(string value, Color color)
        {
            Console.Write(value);
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        public void WriteLine(string value, Color color)
        {
            Console.WriteLine(value);
        }
    }
}
