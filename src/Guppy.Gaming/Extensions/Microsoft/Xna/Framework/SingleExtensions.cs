using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework
{
    public static class SingleExtensions
    {
        public static Vector2 ToVector2(this Single x, Single y = 0)
            => new Vector2(x, y);
    }
}
