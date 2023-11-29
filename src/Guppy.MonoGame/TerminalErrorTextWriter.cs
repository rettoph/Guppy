using Guppy.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    internal class TerminalErrorTextWriter : TerminalTextWriter
    {
        private readonly Ref<Color> _errColor;

        public TerminalErrorTextWriter(Terminal terminal, Ref<Color> errColor) : base(terminal)
        {
            _errColor = errColor;
        }

        protected override void WriteToTerminal(Terminal terminal, char value)
        {
            var color = terminal.Color;

            terminal.Color = _errColor;
            terminal.Write(value);

            terminal.Color = color;
        }

        protected override void WriteLineToTerminal(ITerminal terminal, string value)
        {
            terminal.WriteLine(value, _errColor);
        }

        protected override void WriteToTerminal(ITerminal terminal, string value)
        {
            terminal.Write(value, _errColor);
        }
    }
}
