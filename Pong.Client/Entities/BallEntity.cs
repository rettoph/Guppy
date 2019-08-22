using Guppy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Client.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Client.Entities
{
    public class BallEntity : Entity
    {
        private PongScene _scene;

        public Vector3 Position;
        public Vector3 Velocity;

        public BallEntity(PongScene scene)
        {
            _scene = scene;
        }

        protected override void Initialize()
        {
            base.Initialize();

            var rand = new Random();
            this.Position = new Vector3((float)(rand.NextDouble() * 3) - 1.5f, (float)(rand.NextDouble() * 1) - 0.5f, 0);
            this.Velocity = new Vector3((float)(rand.NextDouble() * 0.1) - 0.05f, (float)(rand.NextDouble() * 0.1) - 0.05f, 0);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Position += this.Velocity;

            if(this.Position.X < -1.5f)
            {
                this.Position.X = -1.5f;
                this.Velocity.X *= -1;
            }
            else if (this.Position.X > 1.5f)
            {
                this.Position.X = 1.5f;
                this.Velocity.X *= -1;
            }

            if (this.Position.Y < -0.5f)
            {
                this.Position.Y = -0.5f;
                this.Velocity.Y *= -1;
            }
            else if (this.Position.X > 0.5f)
            {
                this.Position.Y = 0.5f;
                this.Velocity.Y *= -1;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            for(Int32 i=0; i<16; i++)
            {
                var rads = i * (MathHelper.TwoPi / 16f);

                _scene.LineList.Add(new VertexPositionColor(Position, Color.Red));
                _scene.LineList.Add(new VertexPositionColor(Position + Vector3.Transform(Vector3.UnitX * 0.05f, Matrix.CreateRotationZ(rads)), Color.Red));
            }
        }
    }
}
