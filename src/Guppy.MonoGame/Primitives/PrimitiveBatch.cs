using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.MonoGame.Utilities.Cameras;

namespace Guppy.MonoGame.Primitives
{
    public class PrimitiveBatch<TVertexType>
            where TVertexType : struct, IVertexType
    {
        public static int BufferSize = 500;

        private readonly VertexBuffer _vertexBuffer;
        private readonly IndexBuffer _indexBuffer;
        private readonly TVertexType[] _vertices;
        private readonly short[] _lineIndices;
        private readonly short[] _triangleIndices;
        private readonly short[] _buffer;
        private short _vertexCount;
        private int _lineCount;
        private int _triangleCount;

        public BasicEffect Effect;
        public RasterizerState RasterizerState;
        public BlendState BlendState;
        public readonly GraphicsDevice GraphicsDevice;

        public PrimitiveBatch(GraphicsDevice graphicsDevice)
        {
            _vertexBuffer = new VertexBuffer(graphicsDevice, typeof(TVertexType), BufferSize, BufferUsage.WriteOnly);
            _indexBuffer = new IndexBuffer(graphicsDevice, typeof(short), BufferSize, BufferUsage.WriteOnly);
            _vertices = new TVertexType[BufferSize];
            _lineIndices = new short[BufferSize * 2];
            _triangleIndices = new short[BufferSize * 3];
            _buffer = new short[3];

            this.GraphicsDevice = graphicsDevice;
            this.Effect = new BasicEffect(this.GraphicsDevice)
            {
                VertexColorEnabled = true
            };
            this.BlendState = BlendState.AlphaBlend;
            this.RasterizerState = new RasterizerState()
            {
                MultiSampleAntiAlias = true,
                SlopeScaleDepthBias = 0.5f
            };
        }

        public ref TVertexType NextVertex(out short index)
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

        public void Fill(PrimitiveShape<TVertexType> shape, in Color color, ref Matrix transformation)
        {
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

        public void Trace(PrimitiveShape<TVertexType> shape, in Color color, ref Matrix transformation)
        {
            shape.Transform(0, in color, ref transformation, out this.NextVertex(out _buffer[0]));

            for (int i = 1; i < shape.Length; i++)
            {
                shape.Transform(i, in color, ref transformation, out this.NextVertex(out _buffer[1]));

                this.AddLineIndex(in _buffer[0]);
                this.AddLineIndex(in _buffer[1]);

                _buffer[0] = _buffer[1];
            }
        }

        public void Begin(Camera camera)
        {
            this.Begin(camera.View, camera.Projection);
        }

        public void Begin(Matrix view, Matrix projection)
        {
            this.GraphicsDevice.BlendState = this.BlendState;
            this.GraphicsDevice.RasterizerState = this.RasterizerState;
            this.GraphicsDevice.SetVertexBuffer(_vertexBuffer);
            this.GraphicsDevice.Indices = _indexBuffer;

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
                foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    this.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, _lineCount / 2);
                }
                _lineCount = 0;
            }
        }
    }
}
