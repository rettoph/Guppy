using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    public class OperatorUnit : Unit
    {
        private Boolean _flipped;

        public Unit A { get; set; }
        public Unit B { get; set; }
        public OperatorType Type { get; set; }

        public enum OperatorType
        {
            Add,
            Subtract
        }

        public OperatorUnit(Unit a, Unit b, OperatorType type)
        {
            this.A = a;
            this.B = b;
            this.Type = type;
        }

        public override Int32 ToPixel(Int32 parent)
        {
            switch(this.Type)
            {
                case OperatorType.Add:
                    return this.A.ToPixel(parent) + this.B.ToPixel(parent);
                case OperatorType.Subtract:
                    return this.A.ToPixel(parent) - this.B.ToPixel(parent);
                default:
                    throw new Exception("Invalid Operator Type.");
            }
        }

        public override Unit Flip()
        {
            return new OperatorUnit(this.A, this.B, this.Type)
            {
                _flipped = !_flipped,
            };
        }
    }
}
