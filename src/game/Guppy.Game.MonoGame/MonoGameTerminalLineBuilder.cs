using System.Diagnostics.CodeAnalysis;
using System.Text;
using Guppy.Core.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame
{
    internal class MonoGameTerminalLineBuilder(IRef<Color> color)
    {
        private int _lineNumber = 1;
        private bool _appendLineNumber = true;

        private MonoGameTerminalLine _line = MonoGameTerminalLine.Factory.BuildInstance();

        public IRef<Color> Color = color;

        public StringBuilder Text = new();

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

            if (this._appendLineNumber)
            {
                this.AppendLineNumber();
                this._appendLineNumber = false;
            }

            MonoGameTerminalSegment segment = new(this.Color, this.Text.ToString());
            this._line.Segments.Add(segment);

            this.Text.Clear();
        }

        public MonoGameTerminalLine NewLine()
        {
            this.AddSegment();

            var result = this._line;
            this._line.CleanText();

            this._line = MonoGameTerminalLine.Factory.BuildInstance();
            this._line.Segments.Clear();

            this._appendLineNumber = true;

            return result;
        }

        private void AppendLineNumber()
        {
            MonoGameTerminalSegment segment = new(this.Color, $"{this._lineNumber++}: ");
            this._line.Segments.Add(segment);
        }
    }
}