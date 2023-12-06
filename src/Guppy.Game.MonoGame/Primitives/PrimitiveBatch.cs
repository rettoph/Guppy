using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Game.MonoGame.Utilities.Cameras;

namespace Guppy.Game.MonoGame.Primitives
{
    public class PrimitiveBatch<TVertex, TEffect>
        where TVertex : unmanaged, IVertexType
        where TEffect : Effect, IEffectMatrices
    {
        public static int BufferSize = 8192;

        private readonly DynamicVertexBuffer _vertexBuffer;
        private readonly DynamicIndexBuffer _indexBuffer;
        private readonly TVertex[] _vertices;
        private readonly short[] _lineIndices;
        private readonly short[] _triangleIndices;
        private readonly short[] _buffer;
        private short _vertexCount;
        private int _lineCount;
        private int _triangleCount;

        public TEffect Effect;
        public RasterizerState RasterizerState;
        public BlendState BlendState;
        public readonly GraphicsDevice GraphicsDevice;

        public PrimitiveBatch(GraphicsDevice graphicsDevice, TEffect effect)
        {
            _vertexBuffer = new DynamicVertexBuffer(graphicsDevice, typeof(TVertex), BufferSize, BufferUsage.WriteOnly);
            _indexBuffer = new DynamicIndexBuffer(graphicsDevice, typeof(short), BufferSize * 3, BufferUsage.WriteOnly);
            _vertices = new TVertex[BufferSize];
            _lineIndices = new short[BufferSize * 2];
            _triangleIndices = new short[BufferSize * 3];
            _buffer = new short[3];

            this.GraphicsDevice = graphicsDevice;
            this.Effect = effect;
            this.BlendState = BlendState.AlphaBlend;
            this.RasterizerState = new RasterizerState()
            {
                MultiSampleAntiAlias = true,
                SlopeScaleDepthBias = 0.5f
            };
        }

        public ref TVertex NextVertex(out short index)
        {
            index = _vertexCount++;
            return ref _vertices[index];
        }

        public void AddLineIndex(in short index)
        {
            _lineIndices[_lineCount++] = index;
        }

        public void AddTriangleIndex(in short index)
        {
            _triangleIndices[_triangleCount++] = index;
        }

        public void Fill(IPrimitiveShape<TVertex> shape, in Color color, ref Matrix transformation)
        {
            this.EnsureCapacity(shape.Length);

            shape.Transform(0, in color, ref transformation, out this.NextVertex(out _buffer[0]));
            shape.Transform(1, in color, ref transformation, out this.NextVertex(out _buffer[1]));
            
            for(int i = 2; i<shape.Length; i++)
            {
                shape.Transform(i, in color, ref transformation, out this.NextVertex(out _buffer[2]));
                
                this.AddTriangleIndex(in _buffer[0]);
                this.AddTriangleIndex(in _buffer[1]);
                this.AddTriangleIndex(in _buffer[2]);

                _buffer[1] = _buffer[2];
            }
        }
        public void Fill(IPrimitiveShape<TVertex> shape, Color color, Matrix transformation)
        {
            this.Fill(shape, in color, ref transformation);
        }

        public void Trace(IPrimitiveShape<TVertex> shape, in Color color, ref Matrix transformation)
        {
            this.EnsureCapacity(shape.Length);

            shape.Transform(0, in color, ref transformation, out this.NextVertex(out _buffer[0]));

            for (int i = 1; i < shape.Length; i++)
            {
                shape.Transform(i, in color, ref transformation, out this.NextVertex(out _buffer[1]));

                this.AddLineIndex(in _buffer[0]);
                this.AddLineIndex(in _buffer[1]);

                _buffer[0] = _buffer[1];
            }
        }
        public void Trace(IPrimitiveShape<TVertex> shape, Color color, Matrix transformation)
        {
            this.Trace(shape, in color, ref transformation);
        }

        public void Begin(Camera camera)
        {
            this.Begin(camera.View, camera.Projection);
        }

        public void Begin(Matrix view, Matrix projection)
        {
            this.Effect.View = view;
            this.Effect.Projection = projection;
        }

        public void End()
        {
            this.Flush();
        }

        public void Flush()
        {
            if(_vertexCount == 0)
            {
                return;
            }

            this.GraphicsDevice.BlendState = this.BlendState;
            this.GraphicsDevice.RasterizerState = this.RasterizerState;
            this.GraphicsDevice.SetVertexBuffer(_vertexBuffer);
            this.GraphicsDevice.Indices = _indexBuffer;

            _vertexBuffer.SetData(_vertices, 0, _vertexCount);
            _vertexCount = 0;

            if (_triangleCount != 0)
            {
                _indexBuffer.SetData(_triangleIndices, 0, _triangleCount);
                foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    this.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _triangleCount / 3);
                }
                _triangleCount = 0;
            }

            if(_lineCount != 0)
            {
                _indexBuffer.SetData(_lineIndices, 0, _lineCount);
                foreach (EffectPass pass in this.Effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    this.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, _lineCount / 2);
                }
                _lineCount = 0;
            }
        }

        public void EnsureCapacity(int vertices)
        {
            if(_vertexCount + vertices < BufferSize)
            {
                return;
            }

            SamplerState samplerState = this.GraphicsDevice.SamplerStates[0];
            BlendState blendState = this.GraphicsDevice.BlendState;
            RasterizerState rasterizerState = this.GraphicsDevice.RasterizerState;

            this.Flush();

            this.GraphicsDevice.SamplerStates[0] = samplerState;
            this.GraphicsDevice.BlendState = blendState;
            this.GraphicsDevice.RasterizerState = rasterizerState;
        }
    }

    public class PrimitiveBatch<TVertex> : PrimitiveBatch<TVertex, BasicEffect>
        where TVertex : unmanaged, IVertexType
    {
        public PrimitiveBatch(GraphicsDevice graphicsDevice) : base(
            graphicsDevice, 
            new BasicEffect(graphicsDevice)
            {
                VertexColorEnabled = true
            })
        {
        }
    }
}
