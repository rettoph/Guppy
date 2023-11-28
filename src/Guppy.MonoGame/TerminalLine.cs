using Guppy.Common.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    internal class TerminalLine
    {
        private const int PoolSize = 100;

        public static Factory<TerminalLine> Factory = new Factory<TerminalLine>(() => new TerminalLine(), PoolSize);

        public List<TerminalSegment> Segments = new List<TerminalSegment>();
    }
}
