using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Num = System.Numerics;

namespace Microsoft.Xna.Framework
{
    public static class ColorExtensions
    {
        public static Num.Vector4 ToNumericsVector4(this Color color)
        {
            return color.ToVector4().ToNumericsVector4();
        }
    }
}
