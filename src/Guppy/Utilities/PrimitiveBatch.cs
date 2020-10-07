using Guppy.Utilities.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Guppy.Utilities
{
    public sealed class PrimitiveBatch
    {
        #region Static Fields
        public static Int32 BufferSize { get; private set; } = 500;
        private static Single Zero = 0;
        #endregion

        #region Private Fields
        private VertexPositionColor[] _triangleVertices;
        private Int32 _triangleVerticeCount;
        private VertexPositionColor[] _lineVertices;
        private Int32 _lineVerticeCount;
        private GraphicsDevice _graphics;
        private VertexBuffer _triangleVertexBuffer;
        private VertexBuffer _lineVertexBuffer;
        private BasicEffect _effect;
        private BlendState _blendState;
        private Boolean _started;
        #endregion

        #region Constructor
        public PrimitiveBatch(GraphicsDevice graphicsDevice)
        {
            _graphics = graphicsDevice;
            _effect = new BasicEffect(_graphics);

            _triangleVertices = new VertexPositionColor[PrimitiveBatch.BufferSize * 3];
            _lineVertices = new VertexPositionColor[PrimitiveBatch.BufferSize * 2];

            _triangleVertexBuffer = new VertexBuffer(_graphics, typeof(VertexPositionColor), _triangleVertices.Length, BufferUsage.WriteOnly);
            _lineVertexBuffer = new VertexBuffer(_graphics, typeof(VertexPositionColor), _lineVertices.Length, BufferUsage.WriteOnly);

            _effect.VertexColorEnabled = true;
        }
        #endregion

        #region Add Methods
        public void AddTriangleVertice(ref Color color, ref Single x, ref Single y, ref Single z)
        {
            _triangleVertices[_triangleVerticeCount].Color = color;
            _triangleVertices[_triangleVerticeCount].Position.X = x;
            _triangleVertices[_triangleVerticeCount].Position.Y = y;
            _triangleVertices[_triangleVerticeCount].Position.Z = z;

            _triangleVerticeCount++;
        }

        public void AddLineVertice(ref Color color, ref Single x, ref Single y, ref Single z)
        {
            _lineVertices[_lineVerticeCount].Color = color;
            _lineVertices[_lineVerticeCount].Position.X = x;
            _lineVertices[_lineVerticeCount].Position.Y = y;
            _lineVertices[_lineVerticeCount].Position.Z = z;

            _lineVerticeCount++;
        }
        #endregion

        #region DrawTriangle Methods
        public void DrawTriangle(
            Color c1, Single x1, Single y1, Single z1,
            Color c2, Single x2, Single y2, Single z2,
            Color c3, Single x3, Single y3, Single z3)
        {
            this.AddTriangleVertice(ref c1, ref x1, ref y1, ref z1);
            this.AddTriangleVertice(ref c2, ref x2, ref y2, ref z2);
            this.AddTriangleVertice(ref c3, ref x3, ref y3, ref z3);

            this.TryFlushTriangleVertices();
        }
        public void DrawTriangle(VertexPositionColor v1, VertexPositionColor v2, VertexPositionColor v3)
            => this.DrawTriangle(
                c1: v1.Color, x1: v1.Position.X, y1: v1.Position.Y, z1: v1.Position.Z,
                c2: v2.Color, x2: v2.Position.X, y2: v2.Position.Y, z2: v2.Position.Z,
                c3: v3.Color, x3: v3.Position.X, y3: v3.Position.Y, z3: v3.Position.Z);

        public void DrawTriangle(Color color, Vector3 p1, Vector3 p2, Vector3 p3)
            => this.DrawTriangle(
                c1: color, x1: p1.X, y1: p1.Y, z1: p1.Z,
                c2: color, x2: p2.X, y2: p2.Y, z2: p2.Z,
                c3: color, x3: p3.X, y3: p3.Y, z3: p3.Z);
        public void DrawTriangle(Color c1, Vector3 p1, Color c2, Vector3 p2, Color c3, Vector3 p3)
            => this.DrawTriangle(
                c1: c1, x1: p1.X, y1: p1.Y, z1: p1.Z,
                c2: c2, x2: p2.X, y2: p2.Y, z2: p2.Z,
                c3: c3, x3: p3.X, y3: p3.Y, z3: p3.Z);

        public void DrawTriangle(Color color, Vector2 p1, Vector2 p2, Vector2 p3)
            => this.DrawTriangle(
                c1: color, x1: p1.X, y1: p1.Y, z1: PrimitiveBatch.Zero,
                c2: color, x2: p2.X, y2: p2.Y, z2: PrimitiveBatch.Zero,
                c3: color, x3: p3.X, y3: p3.Y, z3: PrimitiveBatch.Zero);

        public void DrawTriangle(Color c1, Vector2 p1, Color c2, Vector2 p2, Color c3, Vector2 p3)
            => this.DrawTriangle(
                c1: c1, x1: p1.X, y1: p1.Y, z1: PrimitiveBatch.Zero,
                c2: c2, x2: p2.X, y2: p2.Y, z2: PrimitiveBatch.Zero,
                c3: c3, x3: p3.X, y3: p3.Y, z3: PrimitiveBatch.Zero);

        /// <summary>
        /// Draw many triangles in bulk
        /// </summary>
        /// <param name="color"></param>
        /// <param name="vertices"></param>
        public void DrawTriangles(Color color, params Vector2[] vertices)
        {
            Debug.Assert(vertices.Length % 3 == 0);

            for(Int32 i = 0; i<vertices.Length; i++)
                this.DrawTriangle(color, vertices[i++], vertices[i++], vertices[i++]);
        }
        #endregion

        #region DrawLine Methods
        public void DrawLine(
            Color c1, Single x1, Single y1, Single z1,
            Color c2, Single x2, Single y2, Single z2)
        {
            this.AddLineVertice(ref c1, ref x1, ref y1, ref z1);
            this.AddLineVertice(ref c2, ref x2, ref y2, ref z2);

            this.TryFlushLineVertices();
        }

        public void DrawLine(Color color, Vector3 p1, Vector3 p2)
            => this.DrawLine(
                c1: color, x1: p1.X, y1: p1.Y, z1: p1.Z,
                c2: color, x2: p2.X, y2: p2.Y, z2: p2.Z);
        public void DrawLine(Color c1, Vector3 p1, Color c2, Vector3 p2)
            => this.DrawLine(
                c1: c1, x1: p1.X, y1: p1.Y, z1: p1.Z,
                c2: c2, x2: p2.X, y2: p2.Y, z2: p2.Z);

        public void DrawLine(Color color, Vector2 p1, Vector2 p2)
            => this.DrawLine(
                c1: color, x1: p1.X, y1: p1.Y, z1: PrimitiveBatch.Zero,
                c2: color, x2: p2.X, y2: p2.Y, z2: PrimitiveBatch.Zero);

        public void DrawLine(Color c1, Vector2 p1, Color c2, Vector2 p2)
            => this.DrawLine(
                c1: c1, x1: p1.X, y1: p1.Y, z1: PrimitiveBatch.Zero,
                c2: c2, x2: p2.X, y2: p2.Y, z2: PrimitiveBatch.Zero);
        #endregion

        #region DrawPrimitivePath Methods
        public void DrawPrimitive(Primitive primitive, Color color)
            => this.DrawPrimitive(primitive, color, Matrix.Identity);
        public void DrawPrimitive(Primitive primitive, Color color, Matrix transformation)
            => primitive.Draw(color, transformation, this);
        #endregion

        #region Helper Methods
        public void Begin(
            Matrix view,
            Matrix projection,
            BlendState blendState = default(BlendState))
        {
            if (_started)
                throw new Exception("Unable to start PrimitiveBatch, PrimitiveBatch already started.");

            _effect.View = view;
            _effect.Projection = projection;
            _blendState = blendState == default(BlendState) ? BlendState.AlphaBlend : blendState;
            _started = true;
        }

        public void End()
        {
            if (!_started)
                throw new Exception("Unable to end PrimitiveBatch, PrimitiveBatch not started.");

            this.TryFlushTriangleVertices(true);
            this.TryFlushLineVertices(true);
            _started = false;
        }
        #endregion

        #region Flush Methods
        /// <summary>
        /// Attempt to draw the vertices..
        /// </summary>
        /// <param name="force"></param>
        public Boolean TryFlushTriangleVertices(Boolean force = false)
        {
            if (_triangleVerticeCount == _triangleVertices.Length || (_triangleVerticeCount > 0 && force))
            { // Attempt to render the vertices as is...
                _triangleVertexBuffer.SetData<VertexPositionColor>(_triangleVertices, 0, _triangleVerticeCount);
                _graphics.SetVertexBuffer(_triangleVertexBuffer);
                _graphics.BlendState = _blendState;

                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    _graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, _triangleVerticeCount / 3);
                }

                // Reset the vertices count...
                _triangleVerticeCount = 0;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempt to draw the vertices..
        /// </summary>
        /// <param name="force"></param>
        public Boolean TryFlushLineVertices(Boolean force = false)
        {
            if (_lineVerticeCount == _lineVertices.Length || (_lineVerticeCount > 0 && force))
            { // Attempt to render the vertices as is...
                _lineVertexBuffer.SetData<VertexPositionColor>(_lineVertices, 0, _lineVerticeCount);
                _graphics.SetVertexBuffer(_lineVertexBuffer);
                _graphics.BlendState = _blendState;

                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    _graphics.DrawPrimitives(PrimitiveType.LineList, 0, _lineVerticeCount / 2);
                }

                // Reset the vertices count...
                _lineVerticeCount = 0;

                return true;
            }

            return false;
        }
        #endregion
    }
}
