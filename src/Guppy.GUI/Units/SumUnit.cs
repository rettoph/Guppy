using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Units
{
    internal sealed class SumUnit : Unit
    {
        public readonly Unit Addend1;
        public readonly Unit Addend2;

        public SumUnit(Unit addend1, Unit addend2)
        {
            this.Addend1 = addend1;
            this.Addend2 = addend2;
        }

        public override float Calculate(float parent)
        {
            return this.Addend1.Calculate(parent) + this.Addend2.Calculate(parent);
        }

        public override Unit Inverse()
        {
            return new SumUnit(this.Addend1.Inverse(), this.Addend2.Inverse());
        }
    }
}
