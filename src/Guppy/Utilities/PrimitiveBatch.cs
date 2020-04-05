using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities
{
    public sealed class PrimitiveBatch
    {
        #region Static Fields
        public static Int32 BufferSize { get; private set; } = 500;
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

        #region Draw Methods
        public void DrawTriangle(ref VertexPositionColor v1, ref VertexPositionColor v2, ref VertexPositionColor v3)
        {
            if (!_started)
                throw new Exception("Unable to draw primitive, PrimitiveBatch not started.");

            _triangleVertices[_triangleVerticeCount + 0] = v1;
            _triangleVertices[_triangleVerticeCount + 1] = v2;
            _triangleVertices[_triangleVerticeCount + 2] = v3;

            _triangleVerticeCount += 3;

            this.TryFlushTriangleVertices();
        }

        public void DrawTriangle(Vector3 p1, Color c1, Vector3 p2, Color c2, Vector3 p3, Color c3)
        {
            if (!_started)
                throw new Exception("Unable to draw primitive, PrimitiveBatch not started.");

            _triangleVertices[_triangleVerticeCount + 0].Position = p1;
            _triangleVertices[_triangleVerticeCount + 0].Color = c1;
            _triangleVertices[_triangleVerticeCount + 1].Position = p2;
            _triangleVertices[_triangleVerticeCount + 1].Color = c2;
            _triangleVertices[_triangleVerticeCount + 2].Position = p3;
            _triangleVertices[_triangleVerticeCount + 2].Color = c3;

            _triangleVerticeCount += 3;

            this.TryFlushTriangleVertices();
        }

        public void DrawTriangle(Vector2 p1, Color c1, Vector2 p2, Color c2, Vector2 p3, Color c3)
        {
            if (!_started)
                throw new Exception("Unable to draw primitive, PrimitiveBatch not started.");

            _triangleVertices[_triangleVerticeCount + 0].Position.X = p1.X;
            _triangleVertices[_triangleVerticeCount + 0].Position.Y = p1.Y;
            _triangleVertices[_triangleVerticeCount + 0].Color = c1;
            _triangleVertices[_triangleVerticeCount + 1].Position.X = p2.X;
            _triangleVertices[_triangleVerticeCount + 1].Position.Y = p2.Y;
            _triangleVertices[_triangleVerticeCount + 1].Color = c2;
            _triangleVertices[_triangleVerticeCount + 2].Position.X = p3.X;
            _triangleVertices[_triangleVerticeCount + 2].Position.Y = p3.Y;
            _triangleVertices[_triangleVerticeCount + 2].Color = c3;

            _triangleVerticeCount += 3;

            this.TryFlushTriangleVertices();
        }

        public void DrawLine(ref VertexPositionColor v1, ref VertexPositionColor v2)
        {
            if (!_started)
                throw new Exception("Unable to draw primitive, PrimitiveBatch not started.");

            _lineVertices[_lineVerticeCount + 0] = v1;
            _lineVertices[_lineVerticeCount + 1] = v2;

            _lineVerticeCount += 2;

            this.TryFlushLineVertices();
        }

        public void DrawLine(Vector3 p1, Color c1, Vector3 p2, Color c2)
        {
            if (!_started)
                throw new Exception("Unable to draw primitive, PrimitiveBatch not started.");

            _lineVertices[_lineVerticeCount + 0].Position = p1;
            _lineVertices[_lineVerticeCount + 0].Color = c1;
            _lineVertices[_lineVerticeCount + 1].Position = p2;
            _lineVertices[_lineVerticeCount + 1].Color = c2;

            _lineVerticeCount += 2;

            this.TryFlushLineVertices();
        }

        public void DrawLine(Vector2 p1, Color c1, Vector2 p2, Color c2)
        {
            if (!_started)
                throw new Exception("Unable to draw primitive, PrimitiveBatch not started.");

            _lineVertices[_lineVerticeCount + 0].Position.X = p1.X;
            _lineVertices[_lineVerticeCount + 0].Position.Y = p1.Y;
            _lineVertices[_lineVerticeCount + 0].Color = c1;
            _lineVertices[_lineVerticeCount + 1].Position.X = p2.X;
            _lineVertices[_lineVerticeCount + 1].Position.Y = p2.Y;
            _lineVertices[_lineVerticeCount + 1].Color = c2;

            _lineVerticeCount += 2;

            this.TryFlushLineVertices();
        }

        public void DrawLine(Vector2 p1, Vector2 p2, Color c)
        {
            this.DrawLine(p1, c, p2, c);
        }
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
        private Boolean TryFlushTriangleVertices(Boolean force = false)
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
        private Boolean TryFlushLineVertices(Boolean force = false)
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
