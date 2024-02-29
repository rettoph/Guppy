using Guppy.Game.MonoGame.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.MonoGame.Primitives
{
    public class PointPrimitiveBatch<TVertex, TEffect>
        where TVertex : unmanaged, IVertexType
        where TEffect : Effect, IEffectMatrices
    {
        private DynamicVertexBuffer _vertexBuffer;
        private PrimitiveType _type;

        public TEffect Effect;
        public RasterizerState RasterizerState;
        public BlendState BlendState;
        public readonly GraphicsDevice GraphicsDevice;

        public TVertex[] Vertices { get; private set; }

        public PointPrimitiveBatch(GraphicsDevice graphicsDevice, TEffect effect)
        {
            _vertexBuffer = null!;

            this.Vertices = Array.Empty<TVertex>();
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
            _vertexBuffer?.Dispose();
            _vertexBuffer = new DynamicVertexBuffer(this.GraphicsDevice, typeof(TVertex), vertexCount, BufferUsage.WriteOnly);

            this.Vertices = new TVertex[vertexCount];
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

            _vertexBuffer.SetData(this.Vertices, 0, this.Vertices.Length);

            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.GraphicsDevice.DrawPrimitives(PrimitiveType.PointList, 0, this.Vertices.Length);
            }
        }
    }

    public class PointPrimitiveBatch<TVertex> : PointPrimitiveBatch<TVertex, BasicEffect>
        where TVertex : unmanaged, IVertexType
    {
        public PointPrimitiveBatch(GraphicsDevice graphicsDevice) : base(
            graphicsDevice,
            new BasicEffect(graphicsDevice)
            {
                VertexColorEnabled = true
            })
        {
        }
    }
}
