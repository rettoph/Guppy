using Guppy.Game.MonoGame.Common.Utilities.Cameras;
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
            _vertexBuffer = null!;

            Vertices = [];
            GraphicsDevice = graphicsDevice;
            Effect = effect;
            BlendState = BlendState.AlphaBlend;
            RasterizerState = new RasterizerState()
            {
                MultiSampleAntiAlias = true,
                SlopeScaleDepthBias = 0.5f
            };
        }

        public void Initialize(int vertexCount)
        {
            _vertexBuffer?.Dispose();
            _vertexBuffer = new DynamicVertexBuffer(GraphicsDevice, typeof(TVertex), vertexCount, BufferUsage.WriteOnly);

            Vertices = new TVertex[vertexCount];
        }

        public void Draw(Camera camera)
        {
            Draw(camera.View, camera.Projection);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Effect.View = view;
            Effect.Projection = projection;

            GraphicsDevice.BlendState = BlendState;
            GraphicsDevice.RasterizerState = RasterizerState;
            GraphicsDevice.SetVertexBuffer(_vertexBuffer);

            _vertexBuffer.SetData(Vertices, 0, Vertices.Length);

            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.PointList, 0, Vertices.Length);
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
