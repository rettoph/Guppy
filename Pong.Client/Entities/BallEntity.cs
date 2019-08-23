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

        public Color _color;

        public BallEntity(PongScene scene)
        {
            _scene = scene;
        }

        protected override void Initialize()
        {
            base.Initialize();

            var rand = new Random();
            this.Position = new Vector3((float)(rand.NextDouble() * 3) - 1.5f, (float)(rand.NextDouble() * 1) - 0.5f, 0);
            this.Velocity = new Vector3((float)(rand.NextDouble() * 0.05) - 0.025f, (float)(rand.NextDouble() * 0.05) - 0.0251f, 0);

            _color = new Color((Byte)rand.Next(255), (Byte)rand.Next(255), (Byte)rand.Next(255), (Byte)rand.Next(255));
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

            if (this.Position.Y < -1f)
            {
                this.Position.Y = -1f;
                this.Velocity.Y *= -1;
            }
            else if (this.Position.Y > 1f)
            {
                this.Position.Y = 1f;
                this.Velocity.Y *= -1;
            }

            if ((new Random()).Next(500) == 1)
                this.Dispose();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            for(Int32 i=0; i<16; i++)
            {
                var rad1 = i * (MathHelper.TwoPi / 16f);
                var rad2 = (i + 1) * (MathHelper.TwoPi / 16f);

                _scene.LineList.Add(new VertexPositionColor(Position + Vector3.Transform(Vector3.UnitX * 0.05f, Matrix.CreateRotationZ(rad1)), Color.Red));
                _scene.LineList.Add(new VertexPositionColor(Position + Vector3.Transform(Vector3.UnitX * 0.05f, Matrix.CreateRotationZ(rad2)), Color.Red));

                _scene.TriangleList.Add(new VertexPositionColor(Position, _color));
                _scene.TriangleList.Add(new VertexPositionColor(Position + Vector3.Transform(Vector3.UnitX * 0.05f, Matrix.CreateRotationZ(rad1)), _color));
                _scene.TriangleList.Add(new VertexPositionColor(Position + Vector3.Transform(Vector3.UnitX * 0.05f, Matrix.CreateRotationZ(rad2)), _color));
            }
        }
    }
}
