using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Primitives
{
    public abstract class Primitive
    {
        internal abstract void Draw(Color color, Matrix transformation, PrimitiveBatch<VertexPositionColor> primitiveBatch);
    }
}
