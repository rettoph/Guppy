using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysVector4 = System.Numerics.Vector4;

namespace Microsoft.Xna.Framework
{
    public static class Vector4Extensions
    {
        public static unsafe SysVector4 ToNumericsVector4(this Vector4 value)
        {
            return new SysVector4(value.X, value.Y, value.Z, value.W);
        }
    }
}
