using Guppy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Client.Scenes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace Pong.Client.Entities
{
    public class PaddleEntity : Entity
    {
        public RectangleF Bounds;
        private PongScene _scene;

        public PaddleEntity(PongScene scene)
        {
            _scene = scene;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Vector3 topLeft = Vector3.Transform(new Vector3(-1.5f, -1f, 0), (Matrix)this.Configuration.CustomData);
            Vector3 bottomRight = Vector3.Transform(new Vector3(-1.5f + 0.05f, -1f + 0.4f, 0), (Matrix)this.Configuration.CustomData);
            this.Bounds = RectangleF.FromLTRB(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.Bounds.Y < -1f)
                this.Bounds.Y = -1f;

            if (this.Bounds.Y > 1f - 0.4f)
                this.Bounds.Y = 1f - 0.4f;
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _scene.LineList.Add(new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Bottom, 0), Color.Blue));
            _scene.LineList.Add(new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Top, 0), Color.Blue));

            _scene.LineList.Add(new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Bottom, 0), Color.Blue));
            _scene.LineList.Add(new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Top, 0), Color.Blue));

            _scene.LineList.Add(new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Bottom, 0), Color.Blue));
            _scene.LineList.Add(new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Bottom, 0), Color.Blue));

            _scene.LineList.Add(new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Top, 0), Color.Blue));
            _scene.LineList.Add(new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Top, 0), Color.Blue));

            _scene.TriangleList.Add(new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Top, 0), Color.Lerp(Color.Blue, Color.Gray, 0.3f)));
            _scene.TriangleList.Add(new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Bottom, 0), Color.Lerp(Color.Blue, Color.Gray, 0.3f)));
            _scene.TriangleList.Add(new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Top, 0), Color.Lerp(Color.Blue, Color.Gray, 0.3f)));

            _scene.TriangleList.Add(new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Bottom, 0), Color.Lerp(Color.Blue, Color.Gray, 0.3f)));
            _scene.TriangleList.Add(new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Bottom, 0), Color.Lerp(Color.Blue, Color.Gray, 0.3f)));
            _scene.TriangleList.Add(new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Top, 0), Color.Lerp(Color.Blue, Color.Gray, 0.3f)));
        }
    }
}
