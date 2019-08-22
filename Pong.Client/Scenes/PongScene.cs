using Guppy;
using Guppy.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Pong.Client.Scenes
{
    [IsScene]
    public class PongScene : Scene
    {
        private GraphicsDevice _graphicsDevice;
        private BasicEffect _basicEffect;
        private RasterizerState _rasturizeState;

        public List<VertexPositionColor> LineList { get; set; }
        public List<VertexPositionColor> TriangleList { get; set; }

        private Double _ballAdded;

        public PongScene(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _basicEffect = new BasicEffect(_graphicsDevice);
            _basicEffect.World = Matrix.Identity;
            _basicEffect.View = Matrix.Identity;
            _basicEffect.Projection = Matrix.CreateOrthographicOffCenter(-1.6f, 1.6f, 1.1f, -1.1f, 0, 1);
            _basicEffect.VertexColorEnabled = true;

            _rasturizeState = new RasterizerState();
            _rasturizeState.CullMode = CullMode.None;

            this.LineList = new List<VertexPositionColor>();
            this.TriangleList = new List<VertexPositionColor>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.entities.Create("pong:field");
            this.entities.Create("pong:paddle:human");
            this.entities.Create("pong:paddle:ai");
        }

        protected override void Update(GameTime gameTime)
        {
            _ballAdded += gameTime.ElapsedGameTime.TotalMilliseconds;

            if(_ballAdded > 1000)
            {
                this.entities.Create("pong:ball");
                _ballAdded = _ballAdded % 1000;
            }


            this.entities.TryUpdate(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.LineList.Clear();
            this.TriangleList.Clear();

            this.entities.TryDraw(gameTime);

            if (this.TriangleList.Count > 0)
            {
                _graphicsDevice.RasterizerState = _rasturizeState;

                var triangleBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPositionColor), this.TriangleList.Count, BufferUsage.WriteOnly);
                triangleBuffer.SetData<VertexPositionColor>(this.TriangleList.ToArray());

                _graphicsDevice.SetVertexBuffer(triangleBuffer);


                foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, this.LineList.Count / 3);
                }
            }

            if (this.LineList.Count > 0)
            {
                _graphicsDevice.RasterizerState = _rasturizeState;

                var lineBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPositionColor), this.LineList.Count, BufferUsage.WriteOnly);
                lineBuffer.SetData<VertexPositionColor>(this.LineList.ToArray());

                _graphicsDevice.SetVertexBuffer(lineBuffer);


                foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    _graphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, this.LineList.Count / 2);
                }
            }
        }
    }
}
