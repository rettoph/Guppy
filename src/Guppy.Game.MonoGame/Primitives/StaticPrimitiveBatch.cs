using Guppy.Game.MonoGame.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;

namespace Guppy.Game.MonoGame.Primitives
{
    public class StaticPrimitiveBatch<TVertex, TEffect>
        where TVertex : unmanaged, IVertexType
        where TEffect : Effect, IEffectMatrices
    {
        private DynamicVertexBuffer _vertexBuffer;
        private DynamicIndexBuffer _indexBuffer;
        private short[] _indices;
        private int _elementCount;
        private PrimitiveType _type;

        public TEffect Effect;
        public RasterizerState RasterizerState;
        public BlendState BlendState;
        public readonly GraphicsDevice GraphicsDevice;

        public TVertex[] Vertices { get; private set; }
        public readonly IReadOnlyCollection<short> Indices;

        public StaticPrimitiveBatch(GraphicsDevice graphicsDevice, TEffect effect)
        {
            _vertexBuffer = null!;
            _indexBuffer = null!;
            _indices = Array.Empty<short>();

            this.Vertices = Array.Empty<TVertex>();
            this.Indices = new ReadOnlyCollection<short>(_indices);
            this.GraphicsDevice = graphicsDevice;
            this.Effect = effect;
            this.BlendState = BlendState.AlphaBlend;
            this.RasterizerState = new RasterizerState()
            {
                MultiSampleAntiAlias = true,
                SlopeScaleDepthBias = 0.5f
            };
        }

        public void Initialize(int vertexCount, PrimitiveType type, params short[] indices)
        {
            _vertexBuffer?.Dispose();
            _indexBuffer?.Dispose();

            _type = type;
            _indices = indices;
            _elementCount = _type switch
            {
                PrimitiveType.PointList => _indices.Length / 1,
                PrimitiveType.LineList => _indices.Length / 2,
                PrimitiveType.TriangleList => _indices.Length / 3,
                _ => throw new NotImplementedException()
            };
            _vertexBuffer = new DynamicVertexBuffer(this.GraphicsDevice, typeof(TVertex), vertexCount, BufferUsage.WriteOnly);
            _indexBuffer = new DynamicIndexBuffer(this.GraphicsDevice, typeof(short), vertexCount * 3, BufferUsage.WriteOnly);

            this.Vertices = new TVertex[vertexCount];

            _indexBuffer.SetData(_indices, 0, _indices.Length);
        }

        public void Draw(Camera camera)
        {
            this.Draw(camera.View, camera.Projection);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            this.Effect.View = view;
            this.Effect.Projection = projection;

            this.GraphicsDevice.BlendState = this.BlendState;
            this.GraphicsDevice.RasterizerState = this.RasterizerState;
            this.GraphicsDevice.SetVertexBuffer(_vertexBuffer);
            this.GraphicsDevice.Indices = _indexBuffer;

            _vertexBuffer.SetData(this.Vertices, 0, this.Vertices.Length);

            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.GraphicsDevice.DrawIndexedPrimitives(_type, 0, 0, _elementCount);
            }
        }
    }

    public class StaticPrimitiveBatch<TVertex> : StaticPrimitiveBatch<TVertex, BasicEffect>
        where TVertex : unmanaged, IVertexType
    {
        public StaticPrimitiveBatch(GraphicsDevice graphicsDevice) : base(
            graphicsDevice,
            new BasicEffect(graphicsDevice)
            {
                VertexColorEnabled = true
            })
        {
        }
    }
}
