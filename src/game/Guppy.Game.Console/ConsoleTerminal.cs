using Guppy.Core.Common;
using Guppy.Game.Common;
using Guppy.Game.Console.Helpers;
using Microsoft.Xna.Framework;
using System.CommandLine.IO;

namespace Guppy.Game.Console
{
    internal class ConsoleTerminal : SystemConsole, ITerminal
    {
        TextWriter ITerminal.Out => System.Console.Out;

        public IRef<Color> Color { get; set; }


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

        public void Write(string value, IRef<Color> color)
        {
            using (ConsoleHelper.ApplyForegroundColor(color.Value))
            {
                System.Console.Write(value);
            }
        }

        public void WriteLine(string value)
        {
            System.Console.WriteLine(value);
        }

        public void WriteLine(string value, IRef<Color> color)
        {
            using (ConsoleHelper.ApplyForegroundColor(color.Value))
            {
                System.Console.WriteLine(value);
            }
        }
    }
}
