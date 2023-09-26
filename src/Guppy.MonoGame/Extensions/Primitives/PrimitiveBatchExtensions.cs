using Guppy.MonoGame.Primitives;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Extensions.Primitives
{
    public static class PrimitiveBatchExtensions
    {
        private static readonly short[] _buffer = new short[3];

        public static void DrawLine<TVertex>(this PrimitiveBatch<TVertex> batch, TVertex v1, TVertex v2)
            where TVertex : unmanaged, IVertexType
        {
            batch.EnsureCapacity(2);

            batch.NextVertex(out _buffer[0]) = v1;
            batch.NextVertex(out _buffer[1]) = v2;

            batch.AddLineIndex(_buffer[0]);
            batch.AddLineIndex(_buffer[1]);
        }

        public static void DrawTriangle<TVertex>(this PrimitiveBatch<TVertex> batch, TVertex v1, TVertex v2, TVertex v3)
            where TVertex : unmanaged, IVertexType
        {
            batch.EnsureCapacity(3);

            batch.NextVertex(out _buffer[0]) = v1;
            batch.NextVertex(out _buffer[1]) = v2;
            batch.NextVertex(out _buffer[2]) = v3;

            batch.AddTriangleIndex(_buffer[0]);
            batch.AddTriangleIndex(_buffer[1]);
            batch.AddTriangleIndex(_buffer[2]);
        }
    }
}
