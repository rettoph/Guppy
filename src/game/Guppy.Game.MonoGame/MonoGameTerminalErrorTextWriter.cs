using Guppy.Core.Common;
using Guppy.Game.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame
{
    internal class MonoGameTerminalErrorTextWriter(MonoGameTerminal terminal, IRef<Color> errColor) : MonoGameTerminalTextWriter(terminal)
    {
        private readonly IRef<Color> _errColor = errColor;

        protected override void WriteToTerminal(MonoGameTerminal terminal, char value)
        {
            var color = terminal.Color;

            terminal.Color = this._errColor;
            terminal.Write(value);

            terminal.Color = color;
        }

        protected override void WriteLineToTerminal(ITerminal terminal, string value) => terminal.WriteLine(value, this._errColor);

        protected override void WriteToTerminal(ITerminal terminal, string value) => terminal.Write(value, this._errColor);
    }
}