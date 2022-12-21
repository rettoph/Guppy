using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    public class PrimitiveBatch<TVertexType, TEffect>
            where TVertexType : struct, IVertexType
            where TEffect : Effect, IEffectMatrices
    {
        #region Static Fields
        /// <summary>
        /// The maximum buffer size of <see cref="_triangleVertexBuffer"/> and
        /// <see cref="_lineVertexBuffer"/>. This is multiplied by 2 & 3 respectively.
        /// </summary>
        public static int BufferSize { get; private set; } = 500;
        #endregion

        #region Private Fields
        /// <summary>
        /// The current vertices to copy to <see cref="_triangleVertexBuffer"/>
        /// when flushing vertex data to <see cref="_graphics"/>.
        /// </summary>
        private TVertexType[] _triangleVertices;

        /// <summary>
        /// The current count of dirty <see cref="TVertexType"/> instances added
        /// to <see cref="_triangleVertexBuffer"/> since the last vertex flush.
        /// </summary>
        private int _triangleVerticeCount;

        /// <summary>
        /// The current vertices to copy to <see cref="_lineVerticeCount"/>
        /// when flushing vertex data to <see cref="_graphics"/>.
        /// </summary>
        private TVertexType[] _lineVertices;

        /// <summary>
        /// The current count of dirty <see cref="TVertexType"/> instances added
        /// to <see cref="_lineVerticeCount"/> since the last vertex flush.
        /// </summary>
        private int _lineVerticeCount;

        /// <summary>
        /// The primary graphics device to be used when rendering primitives
        /// </summary>
        private GraphicsDevice _graphics;

        /// <summary>
        /// A proper vertex buffer when flushing cached 
        /// <see cref="_triangleVertices"/> data.
        /// </summary>
        private VertexBuffer _triangleVertexBuffer;

        /// <summary>
        /// A proper vertex buffer when flushing cached 
        /// <see cref="_lineVertices"/> data.
        /// </summary>
        private VertexBuffer _lineVertexBuffer;

        /// <summary>
        /// The <see cref="BlendState"/> to be used when
        /// flushing the vertices this batch. This is defined 
        /// within the <see cref="Begin(Matrix, Matrix, BlendState, Matrix?)"/>
        /// invocation and will default to <see cref="BlendState.AlphaBlend"/>.
        /// </summary>
        private BlendState _blendState;

        /// <summary>
        /// Indicates whether or not <see cref="Begin(Matrix, Matrix, BlendState, Matrix?)"/>
        /// has been called and <see cref="End"/> has not been called.
        /// </summary>
        private bool _started;

        /// <summary>
        /// The <see cref="RasterizerState"/> to be used when
        /// flushing primitives to the graphics device.
        /// </summary>
        private RasterizerState _rasterizerState;
        #endregion

        #region Public Properties
        /// <summary>
        /// The <see cref="Effect"/> to be used when flushing 
        /// primitive data. This should implement <see cref="IEffectMatrices"/>
        /// as <see cref="Matrix"/> data is passed into in on a
        /// <see cref="Begin(Matrix, Matrix, BlendState, Matrix?)"/> invocation.
        /// </summary>
        public TEffect Effect { get; private set; }
        #endregion

        #region Constructor
        public PrimitiveBatch(TEffect effect, GraphicsDevice graphicsDevice)
        {
            _graphics = graphicsDevice;
            this.Effect = effect;

            _blendState = BlendState.AlphaBlend;
            _rasterizerState = new RasterizerState()
            {
                MultiSampleAntiAlias = true,
                SlopeScaleDepthBias = 0.5f
            };

            _triangleVertices = new TVertexType[PrimitiveBatch<TVertexType, TEffect>.BufferSize * 3];
            _lineVertices = new TVertexType[PrimitiveBatch<TVertexType, TEffect>.BufferSize * 2];

            _triangleVertexBuffer = new VertexBuffer(_graphics, typeof(TVertexType), _triangleVertices.Length, BufferUsage.WriteOnly);
            _lineVertexBuffer = new VertexBuffer(_graphics, typeof(TVertexType), _lineVertices.Length, BufferUsage.WriteOnly);
        }
        #endregion

        #region Add Methods
        /// <summary>
        /// Add a single triangle vertice.
        /// </summary>
        /// <param name="vertice"></param>
        public void AddTriangleVertice(ref TVertexType vertice)
        {
            _triangleVertices[_triangleVerticeCount] = vertice;
            _triangleVerticeCount++;
        }

        /// <summary>
        /// Add a single line vertice.
        /// </summary>
        /// <param name="vertice"></param>
        public void AddLineVertice(ref TVertexType vertice)
        {
            _lineVertices[_lineVerticeCount] = vertice;
            _lineVerticeCount++;
        }
        #endregion

        #region DrawLine Methods
        /// <summary>
        /// Add a full line to the buffer.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        public void DrawLine(ref TVertexType v1, ref TVertexType v2)
        {
            this.AddLineVertice(ref v1);
            this.AddLineVertice(ref v2);

            this.TryFlushLineVertices();
        }

        /// <summary>
        /// Add a full line to the buffer.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        public void DrawLine(TVertexType v1, TVertexType v2)
            => this.DrawLine(ref v1, ref v2);
        #endregion

        #region DrawDriangle Methods
        /// <summary>
        /// Add a full triangle to the buffer.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        public void DrawTriangle(ref TVertexType v1, ref TVertexType v2, ref TVertexType v3)
        {
            this.AddTriangleVertice(ref v1);
            this.AddTriangleVertice(ref v2);
            this.AddTriangleVertice(ref v3);

            this.TryFlushTriangleVertices();
        }
        /// <summary>
        /// Add a full triangle to the buffer.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        public void DrawTriangle(TVertexType v1, TVertexType v2, TVertexType v3)
            => this.DrawTriangle(ref v1, ref v2, ref v3);
        #endregion

        #region TraceTriangle Methods
        /// <summary>
        /// Add a 3 lines to the buffer, tracing the recieved triangle.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        public void TraceTriangle(ref TVertexType v1, ref TVertexType v2, ref TVertexType v3)
        {
            this.DrawLine(ref v1, ref v2);
            this.DrawLine(ref v2, ref v3);
            this.DrawLine(ref v3, ref v1);
        }

        /// <summary>
        /// Add a 3 lines to the buffer, tracing the recieved triangle.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        public void TraceTriangle(TVertexType v1, TVertexType v2, TVertexType v3)
            => this.TraceTriangle(ref v1, ref v2, ref v3);
        #endregion

        #region Helper Methods
        public void Begin(
            Matrix view,
            Matrix projection,
            BlendState blendState = default(BlendState),
            Matrix? world = default(Matrix?))
        {
            if (_started)
                throw new Exception("Unable to start PrimitiveBatch, PrimitiveBatch already started.");

            // Update the effect's world data.
            this.Effect.View = view;
            this.Effect.Projection = projection;
            this.Effect.World = world ?? Matrix.Identity;

            // https://github.com/labnation/MonoGame/blob/d270be3e800a3955886e817cdd06133743a7e043/MonoGame.Framework/Graphics/Effect/EffectHelpers.cs#L71
            // Line 811
            // _worldViewProjectionParam.SetValue(Matrix.Multiply(Matrix.Multiply(Matrix.Identity, view), projection));
            // _inverseResolution.SetValue(new Vector2(1 / _graphics.Viewport.Width, 1 / _graphics.Viewport.Height));

            _blendState = _blendState ?? blendState;
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

                foreach (EffectPass pass in this.Effect.CurrentTechnique.Passes)
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

                foreach (EffectPass pass in this.Effect.CurrentTechnique.Passes)
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

    public class PrimitiveBatch<TVertexType> : PrimitiveBatch<TVertexType, BasicEffect>
        where TVertexType : struct, IVertexType
    {
        public PrimitiveBatch(GraphicsDevice graphicsDevice) : this(
            new BasicEffect(graphicsDevice)
            {
                VertexColorEnabled = true
            },
            graphicsDevice)
        {
        }

        public PrimitiveBatch(BasicEffect effect, GraphicsDevice graphicsDevice) : base(effect, graphicsDevice)
        {

        }
    }
}
