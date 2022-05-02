using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minnow.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Graphics
{
    public class PrimitiveBatch<TVertexType, TEffect> : IPrimitiveBatch<TVertexType>
        where TVertexType : struct, IVertexType
        where TEffect : Effect, IEffectMatrices
    {
        public const int BufferLength = 512;

        private TEffect _effect;
        private GraphicsDevice _graphics;
        private Buffer<TVertexType> _lines;
        private Buffer<TVertexType> _triangles;
        private VertexBuffer _lineBuffer;
        private VertexBuffer _triangleBuffer;
        private RasterizerState _rasterizerState;
        private BlendState _blendState;
        private bool _started;

        public BlendState BlendState
        {
            get => _blendState;
            set => _blendState = value;
        }

        public RasterizerState RasterizerState
        {
            get => _rasterizerState;
            set => _rasterizerState = value;
        }

        public TEffect Effect
        { 
            get => _effect;
            set => _effect = value;
        }

        public PrimitiveBatch(TEffect effect, GraphicsDevice graphics)
        {
            _started = false;
            _effect = effect;
            _graphics = graphics;
            _lines = new Buffer<TVertexType>(BufferLength * 2);
            _triangles = new Buffer<TVertexType>(BufferLength * 3);
            _lineBuffer = new VertexBuffer(_graphics, typeof(TVertexType), _lines.Length, BufferUsage.WriteOnly);
            _triangleBuffer = new VertexBuffer(_graphics, typeof(TVertexType), _triangles.Length, BufferUsage.WriteOnly);
            _rasterizerState = new RasterizerState()
            {
                MultiSampleAntiAlias = true,
                SlopeScaleDepthBias = 0.5f
            };
            _blendState = BlendState.AlphaBlend;
        }

        public void Begin(ref Matrix view, ref Matrix projection, ref Matrix world)
        {
            if (_started)
            {
                throw new Exception($"Unable to start {nameof(PrimitiveBatch<TVertexType, TEffect>)}, already started.");
            }

            _effect.View = view;
            _effect.Projection = projection;
            _effect.World = world;

            _started = true;
        }

        public void End()
        {
            if(!_started)
            {
                throw new Exception($"Unable to end {nameof(PrimitiveBatch<TVertexType, TEffect>)}, not started.");
            }

            this.FlushTriangles();
            this.FlushLines();

            _started = false;
        }

        public void DrawLines(TVertexType[] vertices)
        {
            if(_lines.Length < _lines.Position + vertices.Length)
            {
                this.FlushLines();
            }

            _lines.AddRange(vertices);
        }

        public void DrawTriangles(TVertexType[] vertices)
        {
            if (_triangles.Length < _triangles.Position + vertices.Length)
            {
                this.FlushTriangles();
            }

            _triangles.AddRange(vertices);
        }

        private void FlushLines()
        {
            if (_lines.Position == 0)
            {
                return;
            }

            _lineBuffer.SetData<TVertexType>(_lines.Array, 0, _lines.Length);
            _graphics.SetVertexBuffer(_lineBuffer);
            _graphics.BlendState = _blendState;
            _graphics.RasterizerState = _rasterizerState;

            foreach(EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphics.DrawPrimitives(PrimitiveType.LineList, 0, _lines.Position / 2);
            }

            _lines.Reset();
        }

        private void FlushTriangles()
        {
            if(_triangles.Position == 0)
            {
                return;
            }

            _triangleBuffer.SetData<TVertexType>(_triangles.Array, 0, _triangles.Length);
            _graphics.SetVertexBuffer(_triangleBuffer);
            _graphics.BlendState = _blendState;
            _graphics.RasterizerState = _rasterizerState;

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, _triangles.Position / 3);
            }

            _triangles.Reset();
        }
    }

    public class PrimitiveBatch<TVertexType> : PrimitiveBatch<TVertexType, BasicEffect>
        where TVertexType : struct, IVertexType
    {
        public PrimitiveBatch(BasicEffect effect, GraphicsDevice graphics) : base(effect, graphics)
        {
            this.Effect.VertexColorEnabled = true;
        }
    }
}
