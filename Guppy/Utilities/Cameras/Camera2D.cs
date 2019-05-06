using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Cameras
{
    /// <summary>
    /// Specific camera implementation used
    /// to generate 2d perspectives
    /// </summary>
    public class Camera2D : Camera
    {
        private Vector2 _position;

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public Rectangle Bounds { get; private set; }
        public Single Zoom { get; private set; }
        private GameWindow _window;

        public Camera2D(GameWindow window) : base()
        {
            _window = window;

            this.Position = Vector2.Zero;
            this.Bounds = new Rectangle(0, 0, _window.ClientBounds.Width, _window.ClientBounds.Height);
            this.Zoom = 1f;

            _window.ClientSizeChanged += this.HandleClientBoundsChanged;
        }

        private void HandleClientBoundsChanged(object sender, EventArgs e)
        {
            this.Bounds = new Rectangle(0, 0, _window.ClientBounds.Width, _window.ClientBounds.Height);
            this.dirty = true;
        }

        protected override void clean()
        {
            this.World = Matrix.CreateTranslation(-this.Position.X, -this.Position.Y, 0)
                * Matrix.CreateScale(this.Zoom)
                * Matrix.CreateTranslation(_window.ClientBounds.Width / 2, _window.ClientBounds.Height / 2, 0);

            this.Projection =
                Matrix.CreateOrthographicOffCenter(
                    this.Bounds.Left,
                    this.Bounds.Right,
                    this.Bounds.Bottom,
                    this.Bounds.Top,
                    0f,
                    1f);
        }

        public void MoveTo(Single x, Single y)
        {
            _position.X = x;
            _position.Y = y;

            this.dirty = true;
        }
        public void MoveTo(Vector2 pos)
        {
            _position.X = pos.X;
            _position.Y = pos.Y;

            this.dirty = true;
        }
        public void MoveTo(ref Vector2 pos)
        {
            _position = pos;

            this.dirty = true;
        }

        public void MoveBy(Int32 x, Int32 y)
        {
            _position.X += x;
            _position.Y += y;

            this.dirty = true;
        }
        public void MoveBy(Vector2 pos)
        {
            _position += pos;

            this.dirty = true;
        }

        public void ZoomBy(Single multiplier)
        {
            this.Zoom *= multiplier;

            this.dirty = true;
        }
        public void ZoomTo(Single value)
        {
            this.Zoom = value;

            this.dirty = true;
        }

        public override void Dispose()
        {
            base.Dispose();

            _window.ClientSizeChanged -= this.HandleClientBoundsChanged;
        }
    }
}
