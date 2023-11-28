using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    internal class TerminalTextWriter : TextWriter
    {
        private readonly Terminal _terminal;

        public TerminalTextWriter(Terminal terminal)
        {
            _terminal = terminal;
        }

        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(char value)
        {
            _terminal.Write(value, Console.ForegroundColor, Console.BackgroundColor);
        }
    }
}
