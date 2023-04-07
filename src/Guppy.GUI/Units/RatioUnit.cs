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

        public override float Calculate(float parent)
        {
            return this.Ratio * parent;
        }

        public override Unit Inverse()
        {
            return new RatioUnit(-this.Ratio);
        }
    }
}
