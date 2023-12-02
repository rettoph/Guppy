using Guppy.Common.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    internal class MonoGameTerminalLine
    {
        private const int PoolSize = 100;

        public static Factory<MonoGameTerminalLine> Factory = new Factory<MonoGameTerminalLine>(() => new MonoGameTerminalLine(), PoolSize);

        public List<MonoGameTerminalSegment> Segments = new List<MonoGameTerminalSegment>();

        public string Text = string.Empty;

        public void CleanText()
        {
            this.Text = string.Join("", this.Segments.Select(x => x.Text));
        }
    }
}
