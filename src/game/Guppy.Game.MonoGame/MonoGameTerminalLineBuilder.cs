using Guppy.Core.Common;
using Microsoft.Xna.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Guppy.Game.MonoGame
{
    internal class MonoGameTerminalLineBuilder
    {
        private int _lineNumber = 1;
        private bool _appendLineNumber;

        private MonoGameTerminalLine _line;

        public IRef<Color> Color;

        public StringBuilder Text;

        public MonoGameTerminalLineBuilder(IRef<Color> color)
        {
            _line = MonoGameTerminalLine.Factory.BuildInstance();
            _appendLineNumber = true;

            this.Text = new StringBuilder();
            this.Color = color;
        }

        public bool TryAppend(char value, [MaybeNullWhen(true)] out MonoGameTerminalLine previousLine)
        {
            if (value == '\n')
            {
                previousLine = this.NewLine();
                return false;
            }

            previousLine = null;
            this.Text.Append(value);

            return true;
        }

        public void SetColor(IRef<Color> color)
        {
            this.AddSegment();
            this.Color = color;
        }

        private void AddSegment()
        {
            if (this.Text.Length == 0)
            {
                return;
            }

            if (_appendLineNumber)
            {
                this.AppendLineNumber();
                _appendLineNumber = false;
            }

            MonoGameTerminalSegment segment = new MonoGameTerminalSegment(this.Color, this.Text.ToString());
            _line.Segments.Add(segment);

            this.Text.Clear();
        }

        public MonoGameTerminalLine NewLine()
        {
            this.AddSegment();

            var result = _line;
            _line.CleanText();

            _line = MonoGameTerminalLine.Factory.BuildInstance();
            _line.Segments.Clear();

            _appendLineNumber = true;

            return result;
        }

        private void AppendLineNumber()
        {
            MonoGameTerminalSegment segment = new MonoGameTerminalSegment(this.Color, $"{_lineNumber++}: ");
            _line.Segments.Add(segment);
        }
    }
}
