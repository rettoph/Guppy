using Guppy.Extensions.Microsoft.Xna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Utilities.Primitives
{
    public class PrimitivePath
    {
        private Vector2[] _segment;

        public Matrix Transformation { get; private set; }
        public Matrix[][] VerticeBounds { get; private set; }
        public Vector2[] Vertices { get; private set; }

        public PrimitivePath(params Vector2[] vertices)
        {
            _segment = new Vector2[8];
            this.Vertices = new Vector2[vertices.Length + 1];
            vertices.CopyTo(this.Vertices, 0);
            this.Vertices[this.Vertices.Length - 1] = this.Vertices[0];

            this.VerticeBounds = new Matrix[vertices.Length + 1][];

            this.SetVerticeBounds(out this.VerticeBounds[0], this.Vertices[0], this.Vertices[1], this.Vertices[this.Vertices.Length - 2]);
            this.SetVerticeBounds(out this.VerticeBounds[this.VerticeBounds.Length - 1], this.Vertices[this.Vertices.Length - 1], this.Vertices[1], this.Vertices[this.Vertices.Length - 2]);
            for (Int32 i=1; i<this.Vertices.Length - 1; i++)
                this.SetVerticeBounds(
                    buffer: out this.VerticeBounds[i],
                    point: this.Vertices[i],
                    next: this.Vertices[i + 1],
                    prev: this.Vertices[i - 1]);
        }

        private void SetVerticeBounds(out Matrix[] buffer, Vector2 point, Vector2 next, Vector2? prev = null)
        {
            var angle = point.Angle(next);
            var delta = prev == null ? MathHelper.Pi : point.Angle(prev.Value, next);
            var average = angle - (delta / 2);

            var triangle1 = TriangleHelper.Solve(A: delta / 2, B: MathHelper.PiOver2, a: 1f);

            buffer = new Matrix[]
            {
                Matrix.CreateScale(triangle1.b) * Matrix.CreateRotationZ(average) * Matrix.CreateTranslation(point.X, point.Y, 0),
                Matrix.CreateScale(triangle1.b) * Matrix.CreateRotationZ(average + MathHelper.Pi) * Matrix.CreateTranslation(point.X, point.Y, 0),
            };
        }

        public void Draw(Single width, Color color, Matrix transformation, PrimitiveBatch primitiveBatch)
        {
            Vector2 halfWidth = new Vector2(width / 2, 0);

            _segment[0] = Vector2.Transform(halfWidth, this.VerticeBounds[0][0] * transformation);
            _segment[1] = Vector2.Transform(halfWidth, this.VerticeBounds[0][1] * transformation);

            for (Int32 i=0; i<this.Vertices.Length - 1; i++)
            {
                _segment[2] = Vector2.Transform(halfWidth, this.VerticeBounds[i + 1][0] * transformation);
                _segment[3] = Vector2.Transform(halfWidth, this.VerticeBounds[i + 1][1] * transformation);

                // primitiveBatch.DrawLine(this.Vertices[i], this.Vertices[i + 1], Color.Purple);
                // primitiveBatch.DrawLine(_segment[0], _segment[2], Color.Purple);
                // primitiveBatch.DrawLine(_segment[1], _segment[3], Color.Purple);

                primitiveBatch.DrawTriangle(color, _segment[0], _segment[2], _segment[3]);
                primitiveBatch.DrawTriangle(color, _segment[1], _segment[0], _segment[3]);

                _segment[0] = _segment[2];
                _segment[1] = _segment[3];
            }
        }
    }
}
