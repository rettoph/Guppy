using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Primitives
{
    public abstract class Primitive
    {
        internal abstract void Draw(Color color, Matrix transformation, PrimitiveBatch primitiveBatch);
    }
}
