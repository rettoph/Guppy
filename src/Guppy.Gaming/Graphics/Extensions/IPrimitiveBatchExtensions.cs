using Guppy.Gaming.Graphics;
using Guppy.Gaming.Graphics.PrimitiveShapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Graphics
{
    public static class IPrimitiveBatchExtensions
    {
        public static void Begin<TVertexType>(this IPrimitiveBatch<TVertexType> primitiveBatch, Matrix view, Matrix projection)
            where TVertexType : struct, IVertexType
        {
            primitiveBatch.Begin(ref view, ref projection, Matrix.Identity);
        }
        public static void Begin<TVertexType>(this IPrimitiveBatch<TVertexType> primitiveBatch, ref Matrix view, ref Matrix projection)
            where TVertexType : struct, IVertexType
        {
            primitiveBatch.Begin(ref view, ref projection, Matrix.Identity);
        }
        public static void Begin<TVertexType>(this IPrimitiveBatch<TVertexType> primitiveBatch, ref Matrix view, ref Matrix projection, Matrix world)
            where TVertexType : struct, IVertexType
        {
            primitiveBatch.Begin(ref view, ref projection, ref world);
        }
        public static void Begin<TVertexType>(this IPrimitiveBatch<TVertexType> primitiveBatch, Matrix view, Matrix projection, Matrix world)
            where TVertexType : struct, IVertexType
        {
            primitiveBatch.Begin(ref view, ref projection, ref world);
        }
        public static void Begin<TVertexType>(this IPrimitiveBatch<TVertexType> primitiveBatch, Camera camera)
            where TVertexType : struct, IVertexType
        {
            primitiveBatch.Begin(ref camera.View, ref camera.Projection, ref camera.World);
        }

        public static void Trace<TVertexType>(this IPrimitiveBatch<TVertexType> primitiveBatch, PrimitiveShape<TVertexType> shape)
            where TVertexType : struct, IVertexType
        {
            shape.Trace(primitiveBatch);
        }
        public static void Fill<TVertexType>(this IPrimitiveBatch<TVertexType> primitiveBatch, PrimitiveShape<TVertexType> shape)
            where TVertexType : struct, IVertexType
        {
            shape.Fill(primitiveBatch);
        }
    }
}
