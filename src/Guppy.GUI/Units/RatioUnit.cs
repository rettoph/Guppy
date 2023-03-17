using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Units
{
    internal sealed class RatioUnit : Unit
    {
        public new readonly float Ratio;

        public RatioUnit(float ratio)
        {
            this.Ratio = ratio;
        }

        public override int Calculate(int parent)
        {
            return (int)(this.Ratio * parent);
        }

        public override Unit Inverse()
        {
            return new RatioUnit(-this.Ratio);
        }
    }
}
