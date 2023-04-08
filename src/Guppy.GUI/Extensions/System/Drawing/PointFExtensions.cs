using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Extensions.System.Drawing
{
    public static class PointFExtensions
    {
        public static Vector2 AsVector2(this PointF point)
        {
            return Unsafe.As<PointF, Vector2>(ref point);
        }

        public static ref Vector2 AsVector2Ref(ref this PointF point)
        {
            return ref Unsafe.As<PointF, Vector2>(ref point);
        }
    }
}
