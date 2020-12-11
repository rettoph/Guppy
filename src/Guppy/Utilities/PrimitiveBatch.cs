using Guppy.Services;
using Guppy.Utilities.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Guppy.Utilities
{
    public sealed class PrimitiveBatch<TVertexType>
        where TVertexType : struct, IVertexType
    {
        #region Static Fields
        public static Int32 BufferSize { get; private set; } = 500;
        #endregion

        #region Private Fields
        private TVertexType[] _triangleVertices;
        private Int32 _triangleVerticeCount;
        private TVertexType[] _lineVertices;
        private Int32 _lineVerticeCount;
        private GraphicsDevice _graphics;
        private VertexBuffer _triangleVertexBuffer;
        private VertexBuffer _lineVertexBuffer;
        private BlendState _blendState;
        private Boolean _started;
        private RasterizerState _rasterizerState;

        private Effect _effect;
        private EffectParameter _worldViewProjectionParam;
        private EffectParameter _inverseResolution;
        #endregion

        #region Constructor
        public PrimitiveBatch(ContentService content, GraphicsDevice graphicsDevice)
        {
            _graphics = graphicsDevice;
            _effect = content.Get<Effect>("effects:primitive-batch-effect");
            _worldViewProjectionParam = _effect.Parameters["WorldViewProjection"];
            _inverseResolution = _effect.Parameters["InverseResolution"];

            _rasterizerState = new RasterizerState()
            {
                MultiSampleAntiAlias = true,
                SlopeScaleDepthBias = 0.5f
            };

            _triangleVertices = new TVertexType[PrimitiveBatch<TVertexType>.BufferSize * 3];
            _lineVertices = new TVertexType[PrimitiveBatch<TVertexType>.BufferSize * 2];

            _triangleVertexBuffer = new VertexBuffer(_graphics, typeof(TVertexType), _triangleVertices.Length, BufferUsage.WriteOnly);
            _lineVertexBuffer = new VertexBuffer(_graphics, typeof(TVertexType), _lineVertices.Length, BufferUsage.WriteOnly);
        }
        #endregion

        #region Add Methods
        public void AddTriangleVertice(ref TVertexType vertice)
        {
            _triangleVertices[_triangleVerticeCount] = vertice;
            _triangleVerticeCount++;
        }

        public void AddLineVertice(ref TVertexType vertice)
        {
            _lineVertices[_lineVerticeCount] = vertice;
            _lineVerticeCount++;
        }
        #endregion

        #region DrawLine Methods
        public void DrawLine(ref TVertexType v1, ref TVertexType v2)
        {
            this.AddLineVertice(ref v1);
            this.AddLineVertice(ref v2);

            this.TryFlushLineVertices();
        }

        public void DrawLine(TVertexType v1, TVertexType v2)
            => this.DrawLine(ref v1, ref v2);
        #endregion

        #region DrawDriangle Methods
        public void DrawTriangle(ref TVertexType v1, ref TVertexType v2, ref TVertexType v3)
        {
            this.AddTriangleVertice(ref v1);
            this.AddTriangleVertice(ref v2);
            this.AddTriangleVertice(ref v3);

            this.TryFlushTriangleVertices();
        }
        public void DrawTriangle(TVertexType v1, TVertexType v2, TVertexType v3)
            => this.DrawTriangle(ref v1, ref v2, ref v3);
        #endregion

        #region TraceTriangle Methods
        public void TraceTriangle(ref TVertexType v1, ref TVertexType v2, ref TVertexType v3)
        {
            this.DrawLine(ref v1, ref v2);
            this.DrawLine(ref v2, ref v3);
            this.DrawLine(ref v3, ref v1);
        }
        public void TraceTriangle(TVertexType v1, TVertexType v2, TVertexType v3)
            => this.TraceTriangle(ref v1, ref v2, ref v3);
        #endregion

        #region Helper Methods
        public void Begin(
            Matrix view,
            Matrix projection,
            BlendState blendState = default(BlendState))
        {
            if (_started)
                throw new Exception("Unable to start PrimitiveBatch, PrimitiveBatch already started.");

            // https://github.com/labnation/MonoGame/blob/d270be3e800a3955886e817cdd06133743a7e043/MonoGame.Framework/Graphics/Effect/EffectHelpers.cs#L71
            // Line 81
            _worldViewProjectionParam.SetValue(Matrix.Multiply(Matrix.Multiply(Matrix.Identity, view), projection));
            // _inverseResolution.SetValue(new Vector2(1 / _graphics.Viewport.Width, 1 / _graphics.Viewport.Height));

            _blendState = blendState ?? BlendState.AlphaBlend;
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
                _triangleVertexBuffer.SetData<TVertexType>(_triangleVertices, 0, _triangleVerticeCount);
                _graphics.SetVertexBuffer(_triangleVertexBuffer);
                _graphics.BlendState = _blendState;
                _graphics.RasterizerState = _rasterizerState;

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
                _lineVertexBuffer.SetData<TVertexType>(_lineVertices, 0, _lineVerticeCount);
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
