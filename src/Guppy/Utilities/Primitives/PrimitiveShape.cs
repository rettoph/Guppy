using Guppy.Extensions.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Guppy.Utilities.Primitives
{
    public class PrimitiveShape : Primitive
    {
        #region Private Fields
        private Vector2[] _buffer;
        #endregion

        #region Public Properties
        public Vector2[] Vertices { get; private set; }
        #endregion

        #region Cosntructor
        private PrimitiveShape(params Vector2[] vertices)
        {
            Debug.Assert(vertices.Length >= 3);

            _buffer = new Vector2[3];
            this.Vertices = vertices;
        }
        #endregion

        #region Primitive Implementation
        internal override void Draw(Color color, Matrix transformation, PrimitiveBatch<VertexPositionColor> primitiveBatch)
        {
            // Pre-calculate & cache the first 2 vertices
            _buffer[0] = Vector2.Transform(this.Vertices[0], transformation);
            _buffer[1] = Vector2.Transform(this.Vertices[1], transformation);
            for (Int32 i=2; i<this.Vertices.Length; i++)
            { // Iterate through each triangle to be drawn...

                // Calculate a new vertice point
                _buffer[2] = Vector2.Transform(this.Vertices[i], transformation);

                // Render the current triangle..
                primitiveBatch.DrawTriangle(color, _buffer[0], _buffer[1], _buffer[2]);

                // primitiveBatch.DrawLine(Color.Red, _buffer[0], _buffer[1]);
                // primitiveBatch.DrawLine(Color.Red, _buffer[1], _buffer[2]);
                // primitiveBatch.DrawLine(Color.Red, _buffer[2], _buffer[0]);

                // Move the old vertice down the buffer 1...
                _buffer[1] = _buffer[2];
            }
        }
        #endregion

        #region Static Methods
        public static PrimitiveShape Create(params Vector2[] vertices)
            => new PrimitiveShape(vertices);

        public static PrimitiveShape Create(IEnumerable<Vector2> vertices)
            => PrimitiveShape.Create(vertices.ToArray());
        #endregion
    }
}
