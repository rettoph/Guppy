using Guppy.Engine.Common;
using Guppy.Game.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame
{
    internal class MonoGameTerminalErrorTextWriter : MonoGameTerminalTextWriter
    {
        private readonly IRef<Color> _errColor;

        public MonoGameTerminalErrorTextWriter(MonoGameTerminal terminal, IRef<Color> errColor) : base(terminal)
        {
            _errColor = errColor;
        }

        protected override void WriteToTerminal(MonoGameTerminal terminal, char value)
        {
            var color = terminal.Color;

            terminal.Color = _errColor.Value;
            terminal.Write(value);

            terminal.Color = color;
        }

        protected override void WriteLineToTerminal(ITerminal terminal, string value)
        {
            terminal.WriteLine(value, _errColor.Value);
        }

        protected override void WriteToTerminal(ITerminal terminal, string value)
        {
            terminal.Write(value, _errColor.Value);
        }
    }
}
