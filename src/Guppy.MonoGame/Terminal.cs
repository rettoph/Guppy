using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Common.Collections;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Configuration;

namespace Guppy.MonoGame
{
    internal class Terminal : ITerminal
    {
        public const int BufferSize = 1 << 10;

        private readonly TerminalTextWriter _writer;

        public readonly Buffer<TerminalLine> Lines = new Buffer<TerminalLine>(BufferSize);
        public TerminalLineBuilder _currentLine = new TerminalLineBuilder();


        public Terminal()
        {
            _writer = new TerminalTextWriter(this);

            Console.SetOut(_writer);
            Console.SetError(_writer);
        }

        public void WriteLine(string value)
        {
            //this.WriteLine(value, _lastColor);
        }

        public void WriteLine(string value, Color color)
        {
            //this.Segments.Add((color, value));
        }

        public void Write(char value, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            if(_currentLine.TryAppend(value, foregroundColor, backgroundColor, out TerminalLine? line) == false)
            {
                this.Lines.Add(line);
            }
        }
    }
}
