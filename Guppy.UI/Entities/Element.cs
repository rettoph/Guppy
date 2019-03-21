using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Guppy.Extensions;
using Guppy.UI.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Guppy.UI.Entities
{
    public class Element : Entity
    {
        protected internal VertexPositionColor[] vertices;

        public Rectangle Bounds { get; private set; }

        public ElementState State { get; private set; }

        public Element(Rectangle bounds, EntityConfiguration configuration, Scene scene, ILogger logger, Alignment alignment = Alignment.TopLeft) : base(configuration, scene, logger)
        {
            this.Bounds = bounds;
            this.State = ElementState.Normal;

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
            var mPos = Mouse.GetState().Position.ToVector2();

            if(mPos.Within(this.Bounds))
            {
                this.State = Mouse.GetState().LeftButton == ButtonState.Pressed ? ElementState.Active : ElementState.Hovered;
            }
            else
            {
                this.State = ElementState.Normal;
            }
        }
    }
}
