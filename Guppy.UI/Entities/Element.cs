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
    public abstract class Element : Entity
    {
        private InputManager _inputManager;
        private Boolean _hasLeftButtonBeenUp;

        protected internal VertexPositionColor[] vertices;

        public Rectangle Bounds { get; private set; }
        public Boolean MouseOver { get; private set; }
        public ElementState State { get; private set; }

        public event EventHandler<Element> OnMouseEnter;
        public event EventHandler<Element> OnMouseExit;
        public event EventHandler<Element> OnMouseDown;
        public event EventHandler<Element> OnMouseUp;

        


        public Element(Rectangle bounds, InputManager inputManager, EntityConfiguration configuration, Scene scene, ILogger logger, Alignment alignment = Alignment.TopLeft) : base(configuration, scene, logger)
        {
            _inputManager = inputManager;

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

            this.State = ElementState.Normal;

            this.SetUpdateOrder(100);
        }

        public override void Update(GameTime gameTime)
        {
            this.MouseOver = this.Bounds.Contains(_inputManager.Mouse.Position);

            if(this.MouseOver && this.State == ElementState.Normal)
            { // If mouse enter...
                this.State = ElementState.Hovered;
                this.OnMouseEnter?.Invoke(this, this);

                _hasLeftButtonBeenUp = _inputManager.Mouse.LeftButton == ButtonState.Released;
            }
            else if(!this.MouseOver && this.State != ElementState.Normal)
            { // If mouse exit...
                this.State = ElementState.Normal;
                this.OnMouseExit?.Invoke(this, this);
            }
            else if(this.MouseOver && this.State == ElementState.Hovered && _inputManager.Mouse.LeftButton == ButtonState.Pressed && _hasLeftButtonBeenUp)
            { // If mouse down...
                this.State = ElementState.Active;
                this.OnMouseDown?.Invoke(this, this);
            }
            else if(this.MouseOver && this.State == ElementState.Active && _inputManager.Mouse.LeftButton == ButtonState.Released)
            { // If mouse up...
                this.State = ElementState.Hovered;
                this.OnMouseUp?.Invoke(this, this);
            }
            else if(this.MouseOver && !_hasLeftButtonBeenUp && _inputManager.Mouse.LeftButton == ButtonState.Released)
            { // If mouse up (when it was down on hover)
                _hasLeftButtonBeenUp = _inputManager.Mouse.LeftButton == ButtonState.Released;
            }
        }
    }
}
