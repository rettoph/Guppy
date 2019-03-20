using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Guppy.UI.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Entities
{
    public class Element : Entity
    {
        protected internal VertexPositionColor[] vertices;

        public Rectangle Bounds { get; private set; }

        public Element(Rectangle bounds, EntityConfiguration configuration, Scene scene, ILogger logger, Alignment alignment = Alignment.TopLeft) : base(configuration, scene, logger)
        {
            this.Bounds = bounds;

            this.vertices = new VertexPositionColor[] {
                new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Top, 0), Color.Red),
                new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Top, 0), Color.Red),
                new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Top, 0), Color.Red),
                new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Bottom, 0), Color.Red),
                new VertexPositionColor(new Vector3(this.Bounds.Right, this.Bounds.Bottom, 0), Color.Red),
                new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Bottom, 0), Color.Red),
                new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Bottom, 0), Color.Red),
                new VertexPositionColor(new Vector3(this.Bounds.Left, this.Bounds.Top, 0), Color.Red),
            };
        }

        public override void Draw(GameTime gameTime)
        {
            // throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            // throw new NotImplementedException();
        }
    }
}
