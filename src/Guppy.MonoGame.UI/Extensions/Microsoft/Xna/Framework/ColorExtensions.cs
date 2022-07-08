using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysVector4 = System.Numerics.Vector4;

namespace Microsoft.Xna.Framework
{
    public static class ColorExtensions
    {
        public static SysVector4 ToNumericsVector4(this Color color)
        {
            return color.ToVector4().ToNumericsVector4();
        }
    }
}
