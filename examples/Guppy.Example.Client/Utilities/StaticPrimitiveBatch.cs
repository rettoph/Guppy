using System.Collections.ObjectModel;
using Guppy.Game.Graphics.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Example.Client.Utilities
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
            this._vertexBuffer = null!;
            this._indexBuffer = null!;
            this._indices = [];

            this.Vertices = [];
            this.Indices = new ReadOnlyCollection<short>(this._indices);
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
            this._vertexBuffer?.Dispose();
            this._indexBuffer?.Dispose();

            this._type = type;
            this._indices = indices;
            this._elementCount = this._type switch
            {
                PrimitiveType.PointList => this._indices.Length / 1,
                PrimitiveType.LineList => this._indices.Length / 2,
                PrimitiveType.TriangleList => this._indices.Length / 3,
                _ => throw new NotImplementedException()
            };
            this._vertexBuffer = new DynamicVertexBuffer(this.GraphicsDevice, typeof(TVertex), vertexCount, BufferUsage.WriteOnly);
            this._indexBuffer = new DynamicIndexBuffer(this.GraphicsDevice, typeof(short), vertexCount * 3, BufferUsage.WriteOnly);

            this.Vertices = new TVertex[vertexCount];

            this._indexBuffer.SetData(this._indices, 0, this._indices.Length);
        }

        public void Draw(ICamera camera) => this.Draw(camera.View, camera.Projection);

        public void Draw(Matrix view, Matrix projection)
        {
            this.Effect.View = view;
            this.Effect.Projection = projection;

            this.GraphicsDevice.BlendState = this.BlendState;
            this.GraphicsDevice.RasterizerState = this.RasterizerState;
            this.GraphicsDevice.SetVertexBuffer(this._vertexBuffer);
            this.GraphicsDevice.Indices = this._indexBuffer;

            this._vertexBuffer.SetData(this.Vertices, 0, this.Vertices.Length);

            foreach (EffectPass pass in this.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.GraphicsDevice.DrawIndexedPrimitives(this._type, 0, 0, this._elementCount);
            }
        }
    }

    public class StaticPrimitiveBatch<TVertex>(GraphicsDevice graphicsDevice) : StaticPrimitiveBatch<TVertex, BasicEffect>(
        graphicsDevice,
        new BasicEffect(graphicsDevice)
        {
            VertexColorEnabled = true
        })
        where TVertex : unmanaged, IVertexType
    {
    }
}