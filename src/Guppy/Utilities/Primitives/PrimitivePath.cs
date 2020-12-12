using Guppy.Extensions.Microsoft.Xna.Framework;
using Guppy.Extensions.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Utilities.Primitives
{
    public class PrimitivePath : Primitive
    {
        #region Private Fields
        /// <summary>
        /// Simple buffer array used as a placeholder when 
        /// calculating any vertices.
        /// </summary>
        private Vector2[] _buffer;

        private Single _width;
        #endregion

        #region Public Properties
        /// <summary>
        /// A multidimensional array of the default transformation to apply to
        /// the vertices based on a width value of 1. This value will scale
        /// appropriately
        /// </summary>
        public Matrix[][] VerticeBounds { get; private set; }


        /// <summary>
        /// The path source, represents a 0 width path.
        /// </summary>
        public Vector2[] Source { get; private set; }

        /// <summary>
        /// A buffer defining every vertice for
        /// each triangle bound pre transformation.
        /// 
        /// This automatically gets recalculated when
        /// the path is updated.
        /// </summary>
        public Vector2[] VerticesBuffer { get; private set; }

        /// <summary>
        /// The current with of the path when drawn.
        /// </summary>
        public Single Width
        {
            get => _width;
            set
            {
                if(_width != value)
                {
                    _width = value;
                    this.CleanVerticeBuffer();
                }
            }
        }
        #endregion

        #region Constructor
        private PrimitivePath(Single width, params Vector2[] vertices)
        {
            _buffer = new Vector2[4];
            this.Source = new Vector2[vertices.Length + 1];
            vertices.CopyTo(this.Source, 0);
            this.Source[this.Source.Length - 1] = this.Source[0];

            this.VerticesBuffer = new Vector2[2 * this.Source.Length];
            this.VerticeBounds = new Matrix[vertices.Length + 1][];
            this.SetVerticeBounds(out this.VerticeBounds[0], this.Source[0], this.Source[1], this.Source[this.Source.Length - 2]);
            this.SetVerticeBounds(out this.VerticeBounds[this.VerticeBounds.Length - 1], this.Source[this.Source.Length - 1], this.Source[1], this.Source[this.Source.Length - 2]);
            for (Int32 i = 1; i < this.Source.Length - 1; i++)
                this.SetVerticeBounds(
                    buffer: out this.VerticeBounds[i],
                    point: this.Source[i],
                    next: this.Source[i + 1],
                    prev: this.Source[i - 1]);

            // Set the width to default.
            this.Width = width;
        }
        #endregion

        #region Methods
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

        private void CleanVerticeBuffer()
        {
            Vector2 halfWidth = new Vector2(this.Width / 2, 0);

            for (Int32 i = 0; i < this.Source.Length; i++)
            {
                this.VerticesBuffer[(2 * i) + 0] = Vector2.Transform(halfWidth, this.VerticeBounds[i][0]);
                this.VerticesBuffer[(2 * i) + 1] = Vector2.Transform(halfWidth, this.VerticeBounds[i][1]);
            }
        }
        #endregion

        #region Frame Methods
        internal override void Draw<TEffect>(Color color, Matrix transformation, PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch)
        {
            _buffer[0] = Vector2.Transform(this.VerticesBuffer[0], transformation);
            _buffer[1] = Vector2.Transform(this.VerticesBuffer[1], transformation);

            for (Int32 i = 2; i < this.VerticesBuffer.Length; i += 2)
            {
                _buffer[2] = Vector2.Transform(this.VerticesBuffer[i + 0], transformation);
                _buffer[3] = Vector2.Transform(this.VerticesBuffer[i + 1], transformation);

                primitiveBatch.DrawTriangle(color, _buffer[0], _buffer[1], _buffer[2]);
                primitiveBatch.DrawTriangle(color, _buffer[3], _buffer[2], _buffer[1]);

                _buffer[0] = _buffer[2];
                _buffer[1] = _buffer[3];
            }
        }
        #endregion

        #region Static Factory Methods
        public static PrimitivePath Create(Single width, params Vector2[] vertices)
            => new PrimitivePath(width, vertices);
        public static PrimitivePath Create(Single width, IEnumerable<Vector2> vertices)
            => PrimitivePath.Create(width, vertices.ToArray());
        #endregion
    }
}
