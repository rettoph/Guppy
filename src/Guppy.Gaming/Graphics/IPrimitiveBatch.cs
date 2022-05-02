using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Graphics
{
    public interface IPrimitiveBatch<TVertexType>
        where TVertexType : struct, IVertexType
    {
        void Begin(ref Matrix view, ref Matrix projection, ref Matrix world);
        void End();

        void DrawLines(TVertexType[] vertices);
        void DrawTriangles(TVertexType[] vertices);
    }
}
