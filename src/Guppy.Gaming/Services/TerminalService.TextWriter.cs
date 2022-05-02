using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Services
{
    internal partial class TerminalService
    {
        private class TerminalTextWriter : TextWriter
        {
            private readonly ITerminalService _terminal;
            private readonly Color _color;
            private string _line;

            public override Encoding Encoding { get; } = Encoding.Default;

            public TerminalTextWriter(ITerminalService terminal, Color color)
            {
                _line = string.Empty;
                _terminal = terminal;
                _color = color;
            }

            public override void Write(char value)
            {
                if(value == '\r')
                {
                    return;
                }

                if(value == '\n')
                {
                    _terminal.WriteLine(_line, _color);
                    _line = string.Empty;
                    return;
                }

                _line += value;
            }
        }
    }
}
