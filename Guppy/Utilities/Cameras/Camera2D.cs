using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Guppy.Utilities.Cameras
{
    /// <summary>
    /// Specific camera implementation used
    /// to generate 2d perspectives
    /// </summary>
    public class Camera2D : Camera
    {
        private GameWindow _window;
        private GraphicsDevice _graphics;
        private Vector2 _position;
        public RectangleF ViewportBounds { get; private set; }

        protected Boolean dirtyViewport;

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public Single Zoom { get; private set; }

        public Camera2D(GraphicsDevice graphics, GameWindow window) : base(graphics)
        {
            _window = window;
            _graphics = graphics;

            this.dirtyViewport = true;

            this.Position = Vector2.Zero;
            this.Zoom = 1f;

            _window.ClientSizeChanged += this.HandleClientBoundsChanged;
        }

        protected override void Update(GameTime gameTime)
        {
            if (this.dirtyViewport)
            {
                this.ViewportBounds = this.buildViewportBounds();
                this.dirtyViewport = false;
                this.dirty = true;
            }

            base.Update(gameTime);
        }

        #region Matrice Build Methods
        protected override void SetWorld(ref Matrix world)
        {
            // world = Matrix.Identity; 
        }

        protected override void SetProjection(ref Matrix projection)
        {
            projection = Matrix.CreateOrthographicOffCenter(
                    this.Position.X - this.ViewportBounds.Width / 2,
                    this.Position.X + this.ViewportBounds.Width / 2,
                    this.Position.Y + this.ViewportBounds.Height / 2,
                    this.Position.Y - this.ViewportBounds.Height / 2,
                    0f,
                    1f)
                * Matrix.CreateScale(this.Zoom);
        }

        protected override void SetView(ref Matrix view)
        {
            // view = Matrix.Identity;
        }

        protected virtual RectangleF buildViewportBounds()
        {
            return new RectangleF(0, 0, _graphics.Viewport.Width, _graphics.Viewport.Height);
        }
        #endregion

        #region Utility Methods
        public void MoveTo(Single x, Single y)
        {
            if (x != _position.X || y != _position.Y)
            {
                _position.X = x;
                _position.Y = y;

                this.dirty = true;
            }
        }
        public void MoveTo(Vector2 pos)
        {
            if (pos.X != _position.X || pos.Y != _position.Y)
            {
                _position.X = pos.X;
                _position.Y = pos.Y;

                this.dirty = true;
            }
        }
        public void MoveTo(ref Vector2 pos)
        {
            if (pos != _position)
            {
                _position = pos;

                this.dirty = true;
            }
        }

        public void MoveBy(Single x, Single y)
        {
            if (x != 0 || y != 0)
            {
                _position.X += x;
                _position.Y += y;

                this.dirty = true;
            }
        }
        public void MoveBy(Vector2 pos)
        {
            if (pos != Vector2.Zero)
            {
                _position += pos;

                this.dirty = true;
            }
        }

        public void ZoomBy(Single multiplier)
        {
            if (multiplier != 1)
            {
                this.Zoom *= multiplier;

                this.dirty = true;
            }
        }
        public void ZoomTo(Single value)
        {
            if (value != this.Zoom)
            {
                this.Zoom = value;

                this.dirty = true;
            }
        }
        #endregion

        #region Event Handlers
        private void HandleClientBoundsChanged(object sender, EventArgs e)
        {
            this.dirtyViewport = true;
        }
        #endregion

        public override void Dispose()
        {
            base.Dispose();

            _window.ClientSizeChanged -= this.HandleClientBoundsChanged;
        }
    }
}
