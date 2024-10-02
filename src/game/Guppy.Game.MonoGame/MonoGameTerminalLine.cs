using Guppy.Core.Common.Collections;

namespace Guppy.Game.MonoGame
{
    internal class MonoGameTerminalLine
    {
        private const int PoolSize = 100;

        public static Factory<MonoGameTerminalLine> Factory = new(() => new MonoGameTerminalLine(), PoolSize);

        public List<MonoGameTerminalSegment> Segments = [];

        public string Text = string.Empty;

        public void CleanText()
        {
            this.Text = string.Join("", this.Segments.Select(x => x.Text));
        }
    }
}
