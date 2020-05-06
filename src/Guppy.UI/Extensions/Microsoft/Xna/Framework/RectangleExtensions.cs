using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Extensions.Microsoft.Xna.Framework
{
    public static class RectangleExtensions
    {
        public static Rectangle Move(this Rectangle point, Point delta)
        {
            point.Location += delta;
            return point;
        }
    }
}
