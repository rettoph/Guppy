using Guppy.GUI.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public abstract class Unit
    {
        public abstract float Calculate(float parent);
        public abstract Unit Inverse();

        public static Unit Ratio(float ratio)
        {
            return new RatioUnit(ratio);
        }

        public static Unit Pixel(int pixels)
        {
            return new PixelUnit(pixels);
        }

        public static implicit operator Unit(int pixels)
        {
            return Unit.Pixel(pixels);
        }

        public static implicit operator Unit(float ratio)
        {
            return Unit.Ratio(ratio);
        }

        public static Unit operator +(Unit addend1, Unit addend2)
        {
            return new SumUnit(addend1, addend2);
        }

        public static Unit operator -(Unit minuend, Unit subtrahend)
        {
            return new SumUnit(minuend, subtrahend.Inverse());
        }
    }
}
