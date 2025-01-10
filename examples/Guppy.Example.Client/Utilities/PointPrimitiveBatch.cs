using Guppy.Game.Graphics.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Example.Client.Utilities
{
    public class PointPrimitiveBatch<TVertex, TEffect>
        where TVertex : unmanaged, IVertexType
        where TEffect : Effect, IEffectMatrices
    {
        private DynamicVertexBuffer _vertexBuffer;

        public TEffect Effect;
        public RasterizerState RasterizerState;
        public BlendState BlendState;
        public readonly GraphicsDevice GraphicsDevice;

        public TVertex[] Vertices { get; private set; }

        public PointPrimitiveBatch(GraphicsDevice graphicsDevice, TEffect effect)
        {
            this._vertexBuffer = null!;

            this.Vertices = [];
            this.GraphicsDevice = graphicsDevice;
            this.Effect = effect;
            this.BlendState = BlendState.AlphaBlend;
            this.RasterizerState = new RasterizerState()
            {
                MultiSampleAntiAlias = true,
                SlopeScaleDepthBias = 0.5f
            };
        }

        public void Initialize(int vertexCount)
        {
            this._vertexBuffer?.Dispose();
            this._vertexBuffer = new DynamicVertexBuffer(this.GraphicsDevice, typeof(TVertex), vertexCount, BufferUsage.WriteOnly);

            this.Vertices = new TVertex[vertexCount];
        }

        public void Draw(ICamera camera)
        {
            this.Draw(camera.View, camera.Projection);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            this.Effect.View = view;
            this.Effect.Projection = projection;

            this.GraphicsDevice.BlendState = this.BlendState;
            this.GraphicsDevice.RasterizerState = this.RasterizerState;
            this.GraphicsDevice.SetVertexBuffer(this._vertexBuffer);

            this._vertexBuffer.SetData(this.Vertices, 0, this.Vertices.Length);

            foreach (EffectPass pass in this.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.GraphicsDevice.DrawPrimitives(PrimitiveType.PointList, 0, this.Vertices.Length);
            }
        }
    }

    public class PointPrimitiveBatch<TVertex>(GraphicsDevice graphicsDevice) : PointPrimitiveBatch<TVertex, BasicEffect>(
        graphicsDevice,
        new BasicEffect(graphicsDevice)
        {
            VertexColorEnabled = true
        })
        where TVertex : unmanaged, IVertexType
    {
    }
}