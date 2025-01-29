using Guppy.Core.Common.Collections;

namespace Guppy.Game.MonoGame
{
    public class MonoGameTerminalLine
    {
        private const int _poolSize = 100;

        public static Factory<MonoGameTerminalLine> Factory = new(() => new MonoGameTerminalLine(), _poolSize);

        public List<MonoGameTerminalSegment> Segments = [];

        public string Text = string.Empty;

        public void Clean()
        {
            this.Text = string.Join("", this.Segments.Select(x => x.Text));
        }

        public void Reset()
        {
            this.Segments.Clear();

            MonoGameTerminalLine.Factory.TryReturn(this);
        }
    }
}