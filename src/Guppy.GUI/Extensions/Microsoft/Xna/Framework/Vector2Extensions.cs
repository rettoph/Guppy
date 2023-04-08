using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Extensions.Microsoft.Xna.Framework
{
    public static class Vector2Extensions
    {
        public static PointF AsPointF(this Vector2 vector)
        {
            return Unsafe.As<Vector2, PointF>(ref vector);
        }

        public static ref PointF AsPointFRef(ref this Vector2 vector)
        {
            return ref Unsafe.As<Vector2, PointF>(ref vector);
        }
    }
}
