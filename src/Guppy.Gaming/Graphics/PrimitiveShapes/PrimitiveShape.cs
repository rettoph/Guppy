using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Graphics.PrimitiveShapes
{
    public abstract class PrimitiveShape<TVertexType>
        where TVertexType : struct, IVertexType
    {
        protected internal abstract void Trace(IPrimitiveBatch<TVertexType> primitiveBatch);
        protected internal abstract void Fill(IPrimitiveBatch<TVertexType> primitiveBatch);
    }
}
