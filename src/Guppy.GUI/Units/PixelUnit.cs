using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Units
{
    internal sealed class PixelUnit : Unit
    {
        public readonly int Pixels;

        public PixelUnit(int pixels)
        {
            Pixels = pixels;
        }

        public override int Calculate(int parent)
        {
            return this.Pixels;
        }

        public override Unit Inverse()
        {
            return new PixelUnit(-this.Pixels);
        }
    }
}
